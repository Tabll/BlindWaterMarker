//此文件参考官方文档编写：https://docs.microsoft.com/zh-cn/windows/uwp/audio-video-camera/process-software-bitmaps-with-opencv
// OpenCVHelper.h

#pragma once

#include <opencv2\core\core.hpp>
#include <opencv2\imgproc\imgproc.hpp>

namespace OpenCVBridge
{
	public ref class OpenCVHelper sealed
	{
	public:
		OpenCVHelper() {}

		//模糊操作
		void Blur(
			Windows::Graphics::Imaging::SoftwareBitmap^ input,
			Windows::Graphics::Imaging::SoftwareBitmap^ output);


	private:
		// helper functions for getting a cv::Mat from SoftwareBitmap
		bool TryConvert(Windows::Graphics::Imaging::SoftwareBitmap^ from, cv::Mat& convertedMat);
		bool GetPointerToPixelData(Windows::Graphics::Imaging::SoftwareBitmap^ bitmap,
			unsigned char** pPixelData, unsigned int* capacity);
	};
}
