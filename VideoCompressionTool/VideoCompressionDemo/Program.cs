using FFmpeg.AutoGen;

namespace VideoCompressionDemo
{
    //internal class Program
    //{
    //    private static unsafe void Main(string[] args)
    //    {
    //        // 初始化FFmpeg
    //        ffmpeg.av_register_all();
    //        ffmpeg.avformat_network_init();

    //        // 打开输入视频文件
    //        AVFormatContext* inputFormatContext = null;
    //        string inputFilePath = "input_video.mp4";
    //        if (ffmpeg.avformat_open_input(&inputFormatContext, inputFilePath, null, null) != 0)
    //        {
    //            Console.WriteLine("无法打开输入视频文件");
    //            return;
    //        }

    //        // 查找视频流
    //        int videoStreamIndex = -1;
    //        for (int i = 0; i < inputFormatContext->nb_streams; i++)
    //        {
    //            if (inputFormatContext->streams[i]->codecpar->codec_type == AVMediaType.AVMEDIA_TYPE_VIDEO)
    //            {
    //                videoStreamIndex = i;
    //                break;
    //            }
    //        }

    //        if (videoStreamIndex == -1)
    //        {
    //            Console.WriteLine("未找到视频流");
    //            return;
    //        }

    //        // 获取输入视频流的编解码器上下文
    //        AVCodecContext* inputCodecContext = inputFormatContext->streams[videoStreamIndex]->codec;

    //        // 打开视频编解码器
    //        AVCodec* inputCodec = ffmpeg.avcodec_find_decoder(inputCodecContext->codec_id);
    //        if (inputCodec == null)
    //        {
    //            Console.WriteLine("无法找到视频编解码器");
    //            return;
    //        }

    //        if (ffmpeg.avcodec_open2(inputCodecContext, inputCodec, null) < 0)
    //        {
    //            Console.WriteLine("无法打开视频编解码器");
    //            return;
    //        }

    //        // 输出视频分辨率
    //        int outputWidth = 1280;
    //        int outputHeight = 720;

    //        // 创建输出视频文件
    //        AVFormatContext* outputFormatContext = null;
    //        string outputFilePath = "output_video.mp4";
    //        if (ffmpeg.avformat_alloc_output_context2(&outputFormatContext, null, null, outputFilePath) != 0)
    //        {
    //            Console.WriteLine("无法创建输出视频文件");
    //            return;
    //        }

    //        // 添加视频流到输出文件
    //        AVCodec* outputCodec = ffmpeg.avcodec_find_encoder(AVCodecID.AV_CODEC_ID_H264);
    //        AVStream* outputVideoStream = ffmpeg.avformat_new_stream(outputFormatContext, outputCodec);
    //        if (outputVideoStream == null)
    //        {
    //            Console.WriteLine("无法创建输出视频流");
    //            return;
    //        }

    //        // 复制输入视频流参数到输出视频流
    //        ffmpeg.avcodec_parameters_copy(outputVideoStream->codecpar, inputCodecContext->codecpar);
    //        outputVideoStream->codecpar->width = outputWidth;
    //        outputVideoStream->codecpar->height = outputHeight;

    //        // 打开视频编码器
    //        AVCodecContext* outputCodecContext = outputVideoStream->codec;
    //        if (ffmpeg.avcodec_open2(outputCodecContext, outputCodec, null) < 0)
    //        {
    //            Console.WriteLine("无法打开视频编码器");
    //            return;
    //        }

    //        // 写入文件头
    //        if ((outputFormatContext->oformat->flags & ffmpeg.AVFMT_NOFILE) == 0)
    //        {
    //            if (ffmpeg.avio_open(&outputFormatContext->pb, outputFilePath, ffmpeg.AVIO_FLAG_WRITE) < 0)
    //            {
    //                Console.WriteLine("无法打开输出视频文件");
    //                return;
    //            }
    //        }

    //        ffmpeg.avformat_write_header(outputFormatContext, null);

    //        // 分配视频帧和缓冲区
    //        AVFrame* frame = ffmpeg.av_frame_alloc();
    //        AVFrame* outputFrame = ffmpeg.av_frame_alloc();
    //        byte* buffer = (byte*)ffmpeg.av_malloc((ulong)ffmpeg.av_image_get_buffer_size(AVPixelFormat.AV_PIX_FMT_YUV420P, outputWidth, outputHeight, 1));

    //        // 循环读取输入视频帧
    //        AVPacket packet;
    //        ffmpeg.av_init_packet(&packet);
    //        while (ffmpeg.av_read_frame(inputFormatContext, &packet) >= 0)
    //        {
    //            if (packet.stream_index == videoStreamIndex)
    //            {
    //                // 解码视频帧
    //                int frameFinished;
    //                ffmpeg.avcodec_decode_video2(inputCodecContext, frame, &frameFinished, &packet);

    //                // 缩放视频帧
    //                ffmpeg.sws_scale(outputCodecContext->sws_ctx, frame->data, frame->linesize, 0, inputCodecContext->height, outputFrame->data, outputFrame->linesize);

    //                // 设置输出视频帧参数
    //                outputFrame->format = (int)outputCodecContext->pix_fmt;
    //                outputFrame->width = outputWidth;
    //                outputFrame->height = outputHeight;

    //                // 编码视频帧
    //                AVPacket outputPacket;
    //                ffmpeg.av_init_packet(&outputPacket);
    //                int gotOutput;
    //                ffmpeg.avcodec_encode_video2(outputCodecContext, &outputPacket, outputFrame, &gotOutput);

    //                // 将编码后的视频帧写入输出文件
    //                if (gotOutput != 0)
    //                {
    //                    outputPacket.stream_index = outputVideoStream->index;
    //                    ffmpeg.av_packet_rescale_ts(&outputPacket, inputCodecContext->time_base, outputVideoStream->time_base);
    //                    ffmpeg.av_write_frame(outputFormatContext, &outputPacket);
    //                }

    //                ffmpeg.av_packet_unref(&outputPacket);
    //            }

    //            ffmpeg.av_packet_unref(&packet);
    //        }

    //        // 写入文件尾
    //        ffmpeg.av_write_trailer(outputFormatContext);

    //        // 清理资源
    //        ffmpeg.av_frame_free(&frame);
    //        ffmpeg.av_frame_free(&outputFrame);
    //        ffmpeg.avcodec_close(inputCodecContext);
    //        ffmpeg.avcodec_close(outputCodecContext);
    //        ffmpeg.avformat_close_input(&inputFormatContext);
    //        if ((outputFormatContext->oformat->flags & ffmpeg.AVFMT_NOFILE) == 0)
    //        {
    //            ffmpeg.avio_closep(&outputFormatContext->pb);
    //        }
    //        ffmpeg.avformat_free_context(outputFormatContext);

    //        // 释放FFmpeg
    //        ffmpeg.avformat_network_deinit();
    //    }

    //    //static void Main(string[] args)
    //    //{
    //    //}
    //}
}