#include "stdafx.h"
#include "FrameProcessor.h"
#include <iostream>
#include <chrono>

FrameProcessor::FrameProcessor(int rows, int cols) {
	zeros = cv::Mat::zeros(rows, cols, CV_8UC4);
}

cv::Mat mask;

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

	cv::imshow("Background", background);

	absdiff(frame, background, mask);

	if (mask.channels() > 1)
		cvtColor(mask, mask, CV_BGR2GRAY);
	
	// cv::GaussianBlur(mask, mask, cv::Size(5, 5), 3.5, 3.5);

	threshold(mask, mask, config[0], 255, cv::THRESH_BINARY);

	//cv::dilate(mask, mask, cv::Mat(), cv::Size(-1, -1), config[3]);
	//cv::erode(mask, mask, cv::Mat(), cv::Size(-1, -1), config[2]);

	zeros.copyTo(result);
	frame.copyTo(result, mask);
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

