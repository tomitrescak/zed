#include "stdafx.h"
#include "FrameProcessor.h"
#include <iostream>
#include <chrono>

#include <opencv2/opencv.hpp>
#include <opencv2/cudaarithm.hpp>

FrameProcessor::FrameProcessor(int rows, int cols) {
	zeros = cv::Mat::zeros(rows, cols, CV_8UC4);
	zerosGpu.upload(zeros);
}

cv::Mat mask;
cv::Mat result;
cv::cuda::GpuMat maskGpu;
cv::cuda::GpuMat resultGpu;

void FrameProcessor::staticDifference(int config[], cv::Mat & frame, cv::Mat & background, cv::Mat & result)
{
	absdiff(frame, background, mask);

	if (mask.channels() > 1)
		cvtColor(mask, mask, CV_BGR2GRAY);

	cv::GaussianBlur(mask, mask, cv::Size(5, 5), 3.5, 3.5);

	threshold(mask, mask, config[0], 255, cv::THRESH_BINARY);
	// result.release();
	zeros.copyTo(result);
	frame.copyTo(result, mask);
}



void FrameProcessor::dilateDifference(int config[], cv::Mat & frame, cv::Mat & background, cv::Mat & result)
{
	// cv::imshow("Background", background);

	absdiff(frame, background, mask);

	if (mask.channels() > 1)
		cvtColor(mask, mask, CV_BGR2GRAY);
	
	cv::GaussianBlur(mask, mask, cv::Size(5, 5), 3.5, 3.5);

	threshold(mask, mask, config[0], 255, cv::THRESH_BINARY);

	cv::dilate(mask, mask, cv::Mat(), cv::Size(-1, -1), config[3]);
	cv::erode(mask, mask, cv::Mat(), cv::Size(-1, -1), config[2]);

	zeros.copyTo(result);
	frame.copyTo(result, mask);
}

cv::Ptr<cv::cuda::Filter> filter = cv::cuda::createGaussianFilter(CV_8UC4, CV_8UC4, cv::Size(5, 5), 3.5, 3.5);

void FrameProcessor::gpuDifference(int config[], cv::cuda::GpuMat & frame, cv::cuda::GpuMat & background, cv::Mat & result)
{
	cv::cuda::absdiff(frame, background, maskGpu);

	// gaussian
	filter->apply(maskGpu, maskGpu);

	// convert to greyscale
	cv::cuda::cvtColor(maskGpu, maskGpu, cv::COLOR_RGBA2GRAY);

	// threshold
	cv::cuda::threshold(maskGpu, maskGpu, config[0], 255, cv::THRESH_BINARY);

	//  dilate erode
	cv::Ptr<cv::cuda::Filter> dilateFilter = cv::cuda::createMorphologyFilter(cv::MORPH_DILATE, mask.type(), cv::Mat(), cv::Size(-1, -1), config[3]);
	dilateFilter->apply(maskGpu, maskGpu);

	cv::Ptr<cv::cuda::Filter> erodeFilter = cv::cuda::createMorphologyFilter(cv::MORPH_ERODE, mask.type(), cv::Mat(), cv::Size(-1, -1), config[2]);
	dilateFilter->apply(maskGpu, maskGpu);

	zerosGpu.copyTo(resultGpu);
	frame.copyTo(resultGpu, maskGpu);

	resultGpu.download(result);
	/*frame.download(result);

	result.copyTo(result, mask);*/

	cv::imshow("Mask", result);
	//cv::imshow("Cuda", result);
}

void FrameProcessor::depthDifference(int config[], cv::Mat & frame, cv::Mat & background_color, cv::Mat & depth, cv::Mat & background_depth, cv::Mat & result)
{
	absdiff(frame, background_color, mask);

	if (mask.channels() > 1)
		cvtColor(mask, mask, CV_BGR2GRAY);

	/*if (depth.channels() > 1)
	cvtColor(depth, depth, CV_BGR2GRAY);*/

	//cv::Mat depthMask;
	//threshold(depth, mask, config[1], 255, cv::THRESH_BINARY);
	//depthMask.copyTo(mask);


	cv::GaussianBlur(mask, mask, cv::Size(5, 5), 3.5, 3.5);

	threshold(mask, mask, config[0], 255, cv::THRESH_BINARY);

	cv::dilate(mask, mask, cv::Mat(), cv::Size(-1, -1), config[3]);
	cv::erode(mask, mask, cv::Mat(), cv::Size(-1, -1), config[2]);

	zeros.copyTo(result);
	frame.copyTo(result, mask);
}

