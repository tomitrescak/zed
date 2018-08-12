#pragma once

#include <opencv/cv.hpp>

class FrameProcessor
{
public:
	cv::Mat zeros;

	FrameProcessor(int rows, int cols);

	void staticDifference(int config[], cv::Mat &frame, cv::Mat &background, cv::Mat &result);
	void dilateDifference(int config[], cv::Mat &frame, cv::Mat &background, cv::Mat &result);
	void depthDifference(int config[], cv::Mat &framee, cv::Mat &background_color, cv::Mat &depthe, cv::Mat &background_depth, cv::Mat &result);
};