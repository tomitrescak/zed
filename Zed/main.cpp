///////////////////////////////////////////////////////////////////////////
//
// Copyright (c) 2017, STEREOLABS.
//
// All rights reserved.
//
// THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS
// "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT
// LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR
// A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT
// OWNER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL,
// SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT
// LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE,
// DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY
// THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT
// (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE
// OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
//
///////////////////////////////////////////////////////////////////////////

/***********************************************************************************************
 ** This sample demonstrates how to use the ZED SDK with OpenCV. 					  	      **
 ** Depth and images are captured with the ZED SDK, converted to OpenCV format and displayed. **
 ***********************************************************************************************/

#include "stdafx.h"

 // ZED includes
#include <sl_zed/Camera.hpp>

// OpenCV includes
#include <opencv2/opencv.hpp>

#include "Zed.hpp"

using namespace sl;

int fps = 0;
auto start = std::chrono::system_clock::now();

void pfps() {
	// count fps
	auto end = std::chrono::system_clock::now();
	std::chrono::duration<double> diff = end - start;

	if (diff.count() > 1) {
		start = std::chrono::system_clock::now();
		std::cout << fps << std::endl;
		fps = 0;
	}
	else {
		fps += 1;
	}
}

int main() {

	int setup[] = { 1, 0, 0, 0, 0 };
	Core::Zed zed{ setup }; // = Core::Zed();
	
	// std::string svo;
	// zed.init_camera(setup, svo);

    // Loop until 'q' is pressed
    char key = ' ';
    while (key != 'q') {

        if (zed.grab()) {

			cv::imshow("Image", zed.result_left_ocv);
            cv::waitKey(10);

			pfps();
        }
    }
    zed.stop();
    return 0;
}

