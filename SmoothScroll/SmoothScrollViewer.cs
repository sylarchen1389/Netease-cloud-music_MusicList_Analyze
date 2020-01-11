using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;


namespace neteas_cloud_music_analyze.SmoothScroll
{
    public class SmoothScrollViewer : ScrollViewer
    {
        /// <summary>
        /// 垂直归一化步进长度
        /// </summary>
        public double VerticalScrollRatio
        {
            get { return (double)GetValue(VerticalScrollRatioProperty); }
            set { SetValue(VerticalScrollRatioProperty, value); }
        }

        //注册VerticalScrollRatio依赖属性
        public static readonly DependencyProperty VerticalScrollRatioProperty =
            DependencyProperty.Register("VerticalScrollRatio", typeof(double), typeof(SmoothScrollViewer), new PropertyMetadata(0.0, new PropertyChangedCallback(V_ScrollRatioChangedCallBack)));

        private static void V_ScrollRatioChangedCallBack(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var scrollViewer = (ScrollViewer)(d);
            if (scrollViewer != null)
            {
                scrollViewer.ScrollToVerticalOffset((double)(e.NewValue) * scrollViewer.ScrollableHeight);
            }
        }

        /// <summary>
        /// 水平归一化步进长度
        /// </summary>
        public double HorizontalScrollRatio
        {
            get { return (double)GetValue(HorizontalScrollRatioProperty); }
            set { SetValue(HorizontalScrollRatioProperty, value); }
        }

        //注册HorizontalScrollRatio依赖属性
        public static readonly DependencyProperty HorizontalScrollRatioProperty =
            DependencyProperty.Register("HorizontalScrollRatio", typeof(double), typeof(SmoothScrollViewer), new PropertyMetadata(0.0, new PropertyChangedCallback(H_ScrollRatioChangedCallBack)));

        private static void H_ScrollRatioChangedCallBack(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var scrollViewer = (ScrollViewer)(d);
            if (scrollViewer != null)
            { 
                scrollViewer.ScrollToHorizontalOffset((double)(e.NewValue) * scrollViewer.ScrollableWidth);
            }
        }
    }
}