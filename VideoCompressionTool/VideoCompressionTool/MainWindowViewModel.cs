using Microsoft.Win32;
using Microsoft.WindowsAPICodePack.Dialogs;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using VideoCompressionTool.Common;

namespace VideoCompressionTool
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        private string _videoFilePath;
        private string _outputDirectory;
        private List<string> _resolutions;
        private string _selectedResolution;

        public event PropertyChangedEventHandler PropertyChanged;

        public string VideoFilePath
        {
            get { return _videoFilePath; }
            set
            {
                _videoFilePath = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(VideoFilePath)));
            }
        }

        public string OutputDirectory
        {
            get { return _outputDirectory; }
            set
            {
                _outputDirectory = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(OutputDirectory)));
            }
        }

        public List<string> Resolutions
        {
            get { return _resolutions; }
            set
            {
                _resolutions = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Resolutions)));
            }
        }

        public string SelectedResolution
        {
            get { return _selectedResolution; }
            set
            {
                _selectedResolution = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SelectedResolution)));
            }
        }

        public ICommand BrowseCommand { get; private set; }
        public ICommand BrowseOutputCommand { get; private set; }
        public ICommand CompressCommand { get; private set; }

        public MainWindowViewModel()
        {
            BrowseCommand = new RelayCommand(BrowseVideo);
            BrowseOutputCommand = new RelayCommand(BrowseOutputDirectory);
            CompressCommand = new RelayCommand(CompressVideo);

            Resolutions = new List<string>
            {
                "1280x720",
                "640x480",
                "320x240"
            };
        }

        private void BrowseVideo()
        {
            // 弹出文件选择对话框，选择视频文件
            // 将选中的视频文件路径赋值给VideoFilePath属性
            var openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Video Files (*.mp4, *.mov, *.avi)|*.mp4;*.mov;*.avi";
            if (openFileDialog.ShowDialog() == true)
            {
                VideoFilePath = openFileDialog.FileName;
            }
        }

        private void BrowseOutputDirectory()
        {
            // 弹出文件夹选择对话框，选择输出路径
            // 将选中的输出路径赋值给OutputDirectory属性
            //var dialog = new OpenFileDialog();
            //if (dialog.ShowDialog() == true)
            //{
            //    OutputDirectory = dialog.FileName;
            //    // 在这里可以使用选定的文件夹路径进行后续操作，例如保存文件等
            //}
            var dialog = new CommonOpenFileDialog();
            dialog.IsFolderPicker = true;
            if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                OutputDirectory = dialog.FileName;
                // 在这里可以使用选定的文件夹路径进行后续操作，例如保存文件等
            }

        }

        private void CompressVideo()
        {
            // 使用FFmpeg进行视频压缩
            string resolution = SelectedResolution;
            string outputFilePath = Path.Combine(OutputDirectory, "compressed.mp4");

            Process process = new Process();
            process.StartInfo.FileName = "ffmpeg";
            process.StartInfo.Arguments = $"-i {VideoFilePath} -s {resolution} {outputFilePath}";
            process.Start();
        }
    }
}
