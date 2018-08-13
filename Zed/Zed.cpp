#include "stdafx.h"
#include "Zed.hpp"
#include <iostream>

// ZED includes
#include <sl_zed/Camera.hpp>

// OpenCV includes
#include <opencv2/opencv.hpp>

#include <opencv2/cudaarithm.hpp>



cv::Mat slMat2cvMat(sl::Mat& input);
cv::cuda::GpuMat slMat2cvCudaMat(sl::Mat& input);

bool file_exist(const char *file) {
	std::ifstream infile(file);
	return infile.good();
}

namespace Core
{
	void Zed::reset_background()
	{
		// cuda

		/*frame_left_cuda.copyTo(background_left_cuda);
		frame_right_cuda.copyTo(background_right_cuda);

		cv::imwrite("left_color.tiff", background_left_cuda);
		cv::imwrite("right_color.tiff", background_right_cuda);

		return;*/

		// normal

		frame_color_left_ocv.copyTo(background_color_left_ocv);
		frame_color_right_ocv.copyTo(background_color_right_ocv);

		cv::imwrite("left_color.tiff", background_color_left_ocv);
		cv::imwrite("right_color.tiff", background_color_right_ocv);

		if (sideBySide) {

			cv::hconcat(background_color_left_ocv, background_color_right_ocv, background_color_left_ocv);
			cv::imwrite("side_by_side.tiff", background_color_left_ocv);
			return;
		}

		if (processDepth) {
			frame_depth_left_ocv.copyTo(background_depth_left_ocv);
			frame_depth_right_ocv.copyTo(background_depth_right_ocv);

			cv::imwrite("left_depth.tiff", background_depth_left_ocv);
			cv::imwrite("right_depth.tiff", background_depth_right_ocv);
		}
	}

	void Zed::init_processor()
	{
	}

	Zed::Zed(int setup[]) {

		// init resolution
		sl::RESOLUTION resolution = static_cast<sl::RESOLUTION>(setup[0]);
		sl::DEPTH_MODE depth = static_cast<sl::DEPTH_MODE>(setup[1]);
		sl::SENSING_MODE sensing = static_cast<sl::SENSING_MODE>(setup[2]);
		sideBySide = setup[3] == 1;
		processDepth = depth != sl::DEPTH_MODE_NONE;
		int sizeDivision = sideBySide ? 1 : 2;

		// Set configuration parameters
		sl::InitParameters init_params;
		init_params.camera_resolution = resolution;
		init_params.depth_mode = depth; // DEPTH_MODE_PERFORMANCE;
		init_params.coordinate_units = sl::UNIT_METER;
		// init_params.enable_right_side_measure = true;

		/*if (!svo.empty()) {
			init_params.svo_input_filename.set(svo.c_str());
		}*/

		// Open the camera
		sl::ERROR_CODE err = zed.open(init_params);
		if (err != sl::SUCCESS) {
			printf("%s\n", toString(err).c_str());
			zed.close();
			return; // Quit if an error occurred
		}

		// Set runtime parameters after opening the camera
		runtime_parameters.sensing_mode = sensing;

		// Prepare new image size to retrieve half-resolution images
		sl::Resolution image_size = zed.getResolution();
		new_width = image_size.width / sizeDivision;
		new_height = image_size.height / sizeDivision;

		// HELPERS

		mask_ocv = cv::Mat::zeros(new_height, new_width, CV_8UC1);
		zeros = cv::Mat::zeros(new_height, new_width, CV_8UC4);

		// color LEFT / SIDE-BY-SIDE

		frame_left_zed = new sl::Mat(new_width, new_height, sl::MAT_TYPE_8U_C4);
		frame_color_left_ocv = slMat2cvMat(*frame_left_zed);

		// color RIGHT

		frame_right_zed = new sl::Mat(new_width, new_height, sl::MAT_TYPE_8U_C4);
		frame_color_right_ocv = slMat2cvMat(*frame_right_zed);

		// CUDA

		frame_left_zed_gpu = new sl::Mat(new_width, new_height, sl::MAT_TYPE_8U_C4);
		frame_left_cuda = slMat2cvCudaMat(*frame_left_zed_gpu);

		frame_right_zed_gpu = new sl::Mat(new_width, new_height, sl::MAT_TYPE_8U_C4);
		frame_right_cuda = slMat2cvCudaMat(*frame_right_zed_gpu);


		if (sideBySide) {
			// side-by-side is only using color image
			return;
		}

		// depth LEFT

		if (processDepth) {
			depth_image_left_zed = new sl::Mat(new_width, new_height, sl::MAT_TYPE_8U_C4);
			frame_depth_left_ocv = slMat2cvMat(*depth_image_left_zed);
			depth_image_right_zed = new sl::Mat(new_width, new_height, sl::MAT_TYPE_8U_C4);
			frame_depth_right_ocv = slMat2cvMat(*depth_image_left_zed);
		}
	}

	Zed::~Zed()
	{
	}

	cv::Ptr<cv::cuda::Filter> filter =
		cv::cuda::createGaussianFilter(CV_8UC4, CV_8UC4, cv::Size(5, 5), 3.5, 3.5);

	bool Zed::grab()
	{
		if (zed.grab(runtime_parameters) == sl::SUCCESS) {
			
			// cuda experiment
			

			/*zed.retrieveImage(*frame_left_zed_gpu, VIEW_LEFT, MEM_GPU, new_width, new_height);
			zed.retrieveImage(*frame_right_zed_gpu, VIEW_RIGHT, MEM_GPU, new_width, new_height);

			cv::cuda::absdiff(frame_left_cuda, frame_right_cuda, mask);*/

			// Retrieve the left image, depth image in half-resolution
			
			zed.retrieveImage(*frame_left_zed, sl::VIEW_LEFT, sl::MEM_CPU, new_width, new_height);
			zed.retrieveImage(*frame_right_zed, sl::VIEW_RIGHT, sl::MEM_CPU, new_width, new_height);
			
			// CUDA EXPERIMETNT
			frame_left_cuda.upload(frame_color_left_ocv);
			frame_right_cuda.upload(frame_color_right_ocv);

			if (!init) {
				this->processor = new FrameProcessor{ new_height, new_width };
				frame_left_cuda.copyTo(background_left_cuda);
				frame_right_cuda.copyTo(background_right_cuda);
				init = true;
				return false;
			}

			this->processor->gpuDifference(config, frame_left_cuda, background_left_cuda, result_left_ocv);

			//// difference
			//cv::cuda::absdiff(frame_left_cuda, background_left_cuda, mask);		
			//
			//// gaussian
			//filter->apply(mask, mask);

			//// convert to greyscale
			//cv::cuda::cvtColor(mask, mask, cv::COLOR_RGBA2GRAY);

			//// threshold
			//cv::cuda::threshold(mask, mask, config[0], 255, cv::THRESH_BINARY);

			////  dilate erode
			//cv::Ptr<cv::cuda::Filter> dilateFilter = cv::cuda::createMorphologyFilter(cv::MORPH_DILATE, mask.type(), cv::Mat(), cv::Size(-1, -1), config[3]);
			//dilateFilter->apply(mask, mask);

			//cv::Ptr<cv::cuda::Filter> erodeFilter = cv::cuda::createMorphologyFilter(cv::MORPH_ERODE, mask.type(), cv::Mat(), cv::Size(-1, -1), config[2]);
			//dilateFilter->apply(mask, mask);

			//cv::Mat m;
			//mask.download(m);
			//cv::imshow("Cuda", m);

			return true;

			// myFrame.upload(frame_color_left_ocv);

			if (processDepth) {
				zed.retrieveImage(*depth_image_left_zed, sl::VIEW_DEPTH, sl::MEM_CPU, new_width, new_height);
				zed.retrieveImage(*depth_image_right_zed, sl::VIEW_DEPTH, sl::MEM_CPU, new_width, new_height);
			}

			if (!init) {
				this->reset_background();

				this->processor = new FrameProcessor{ new_height, new_width };
				init = true;
				return false;
			}

			if (processDepth) {
				processor->depthDifference(config, frame_color_left_ocv, background_color_left_ocv, frame_depth_left_ocv, background_depth_left_ocv, result_left_ocv);
				processor->depthDifference(config, frame_color_right_ocv, background_color_right_ocv, frame_depth_right_ocv, background_depth_right_ocv, result_right_ocv);
			}
			else {
				processor->dilateDifference(config, frame_color_left_ocv, background_color_left_ocv, result_left_ocv);
				processor->dilateDifference(config, frame_color_right_ocv, background_color_right_ocv, result_right_ocv);
			}

			// contrast
			if (config[4] != 0) {
				result_left_ocv.convertTo(result_left_ocv, -1, config[4] / (float) 50, 0);
				result_right_ocv.convertTo(result_right_ocv, -1, config[4] / (float) 50, 0);
			}


			// Retrieve the RGBA point cloud in half-resolution
			// To learn how to manipulate and display point clouds, see Depth Sensing sample
			// zed.retrieveMeasure(point_cloud, MEASURE_XYZRGBA, MEM_CPU, new_width, new_height);

			// Display image and depth using cv:Mat which share sl:Mat data
			// cv::imshow("Left", result_left_ocv);
			// cv::imshow("Right", result_right_ocv);

			// Handle key event
			
			return true;
		}

		return false;

	}

	void Zed::stop()
	{
		delete frame_left_zed;
		delete frame_right_zed;
		delete depth_image_left_zed;
		delete depth_image_right_zed;
	}
}

/**
* Conversion function between sl::Mat and cv::Mat
**/
cv::Mat slMat2cvMat(sl::Mat& input) {
	// Mapping between MAT_TYPE and CV_TYPE
	int cv_type = -1;
	switch (input.getDataType()) {
	case sl::MAT_TYPE_32F_C1: cv_type = CV_32FC1; break;
	case sl::MAT_TYPE_32F_C2: cv_type = CV_32FC2; break;
	case sl::MAT_TYPE_32F_C3: cv_type = CV_32FC3; break;
	case sl::MAT_TYPE_32F_C4: cv_type = CV_32FC4; break;
	case sl::MAT_TYPE_8U_C1: cv_type = CV_8UC1; break;
	case sl::MAT_TYPE_8U_C2: cv_type = CV_8UC2; break;
	case sl::MAT_TYPE_8U_C3: cv_type = CV_8UC3; break;
	case sl::MAT_TYPE_8U_C4: cv_type = CV_8UC4; break;
	default: break;
	}

	// Since cv::Mat data requires a uchar* pointer, we get the uchar1 pointer from sl::Mat (getPtr<T>())
	// cv::Mat and sl::Mat will share a single memory structure
	return cv::Mat(input.getHeight(), input.getWidth(), cv_type, input.getPtr<sl::uchar1>(sl::MEM_CPU));
}

/**
* Conversion function between sl::Mat and cv::Mat
**/
cv::cuda::GpuMat slMat2cvCudaMat(sl::Mat& input) {
	// Mapping between MAT_TYPE and CV_TYPE
	int cv_type = -1;
	switch (input.getDataType()) {
	case sl::MAT_TYPE_32F_C1: cv_type = CV_32FC1; break;
	case sl::MAT_TYPE_32F_C2: cv_type = CV_32FC2; break;
	case sl::MAT_TYPE_32F_C3: cv_type = CV_32FC3; break;
	case sl::MAT_TYPE_32F_C4: cv_type = CV_32FC4; break;
	case sl::MAT_TYPE_8U_C1: cv_type = CV_8UC1; break;
	case sl::MAT_TYPE_8U_C2: cv_type = CV_8UC2; break;
	case sl::MAT_TYPE_8U_C3: cv_type = CV_8UC3; break;
	case sl::MAT_TYPE_8U_C4: cv_type = CV_8UC4; break;
	default: break;
	}

	// Since cv::Mat data requires a uchar* pointer, we get the uchar1 pointer from sl::Mat (getPtr<T>())
	// cv::Mat and sl::Mat will share a single memory structure
	return cv::cuda::GpuMat(input.getHeight(), input.getWidth(), cv_type, input.getPtr<sl::uchar1>(sl::MEM_GPU));
}

void * create_zed(int setup[])
{
	return new Core::Zed(setup);
}

void init_camera(void * zed, int setup[], std::string svo)
{
}

void destroy_zed(void * zed)
{
	Core::Zed* zed_ptr = reinterpret_cast<Core::Zed*>(zed);
	zed_ptr->stop();
	delete zed_ptr;
}

bool grab_zed(void * zed)
{
	Core::Zed* zed_ptr = reinterpret_cast<Core::Zed*>(zed);
	return zed_ptr->grab();
}

void stop_zed(void * zed)
{
	Core::Zed* zed_ptr = reinterpret_cast<Core::Zed*>(zed);
	zed_ptr->stop();
}

void * get_left(void * zed)
{
	Core::Zed* zed_ptr = reinterpret_cast<Core::Zed*>(zed);
	return zed_ptr->result_left_ocv.data;
}

void * get_right(void * zed)
{
	Core::Zed* zed_ptr = reinterpret_cast<Core::Zed*>(zed);
	return zed_ptr->result_right_ocv.data;
}

void reset_background(void * zed)
{
	Core::Zed* zed_ptr = reinterpret_cast<Core::Zed*>(zed);
	return zed_ptr->reset_background();
}

void setup(void * zed, int setup[])
{
	Core::Zed* zed_ptr = reinterpret_cast<Core::Zed*>(zed);
	zed_ptr->config[0] = setup[0];
	zed_ptr->config[1] = setup[1];
	zed_ptr->config[2] = setup[2];
	zed_ptr->config[3] = setup[3];
	zed_ptr->config[4] = setup[4];
}
