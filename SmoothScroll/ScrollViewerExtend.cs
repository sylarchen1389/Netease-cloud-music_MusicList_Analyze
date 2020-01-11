using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Media.Animation;
using neteas_cloud_music_analyze;


namespace neteas_cloud_music_analyze.SmoothScroll
{
    public static class ScrollViewerExtend
    {
        /// <summary>
        /// 实现ScrollViewer的平滑滚动
        /// </summary>
        /// <param name="scrollViewer"></param>
        /// <param name="ScrollStepRatio">归一化步进长度</param>
        /// <param name="ScrollPositionRatio">归一化位置</param>
        /// <param name="direction">滚动方向</param>
        public static void SmoothScroll(this SmoothScrollViewer scrollViewer, double ScrollStepRatio, double ScrollPositionRatio, ScrollDirection direction)
        {
            if (double.IsNaN(ScrollStepRatio) || double.IsNaN(ScrollPositionRatio))
                return;
            DoubleAnimation Animation = new DoubleAnimation();

            Animation.From = ScrollPositionRatio;
            if (ScrollDirection.Down == direction || ScrollDirection.Right == direction)
            {
                double To = ScrollPositionRatio + ScrollStepRatio;
                Animation.To = To > 0.95 ? 1.0 : To;//向下（右）滚动补偿
            }
            else if (ScrollDirection.Up == direction || ScrollDirection.Left == direction)
            {
                double To = ScrollPositionRatio - ScrollStepRatio;
                Animation.To = To < 0.05 ? 0.0 : To;//向上（左）滚动补偿
            }
            Animation.Duration = new Duration(TimeSpan.FromMilliseconds(1000));

            Storyboard storyboard = new Storyboard();
            storyboard.Children.Add(Animation);
            Storyboard.SetTarget(Animation, scrollViewer);
            if (ScrollDirection.Down == direction || ScrollDirection.Up == direction)
                Storyboard.SetTargetProperty(Animation, new PropertyPath(SmoothScrollViewer.VerticalScrollRatioProperty));
            else if (ScrollDirection.Right == direction || ScrollDirection.Left == direction)
                Storyboard.SetTargetProperty(Animation, new PropertyPath(SmoothScrollViewer.HorizontalScrollRatioProperty));
            storyboard.Begin();
        }
    }

    public enum ScrollDirection
    {
        Up, Down, Left, Right
    }
}