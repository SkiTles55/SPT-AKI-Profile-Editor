using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace SPT_AKI_Profile_Editor.Views.ExtendedControls
{
    internal class AlignableWrapPanel : Panel
    {
        public static readonly DependencyProperty HorizontalContentAlignmentProperty =
            DependencyProperty.Register(nameof(HorizontalContentAlignment),
                                        typeof(HorizontalAlignment),
                                        typeof(AlignableWrapPanel),
                                        new FrameworkPropertyMetadata(HorizontalAlignment.Left, FrameworkPropertyMetadataOptions.AffectsArrange));

        public HorizontalAlignment HorizontalContentAlignment
        {
            get { return (HorizontalAlignment)GetValue(HorizontalContentAlignmentProperty); }
            set { SetValue(HorizontalContentAlignmentProperty, value); }
        }

        protected override Size MeasureOverride(Size constraint)
        {
            Size curLineSize = new();
            Size panelSize = new();

            foreach (UIElement child in InternalChildren)
            {
                // Flow passes its own constraint to children
                child.Measure(constraint);
                Size sz = child.DesiredSize;

                if (curLineSize.Width + sz.Width > constraint.Width)
                {
                    //need to switch to another line
                    panelSize = AddNewLine(curLineSize, panelSize);
                    curLineSize = sz;

                    if (sz.Width > constraint.Width)
                    {
                        // if the element is wider then the constraint - give it a separate line
                        panelSize = AddNewLine(sz, panelSize);
                        curLineSize = new Size();
                    }
                }
                else //continue to accumulate a line
                    curLineSize = ContinueLine(curLineSize, sz);
            }

            // the last line size, if any need to be added
            panelSize = AddNewLine(curLineSize, panelSize);

            return panelSize;
        }

        protected override Size ArrangeOverride(Size arrangeBounds)
        {
            int firstInLine = 0;
            Size curLineSize = new();
            double accumulatedHeight = 0;
            UIElementCollection children = InternalChildren;

            for (int i = 0; i < children.Count; i++)
            {
                Size sz = children[i].DesiredSize;

                if (curLineSize.Width + sz.Width > arrangeBounds.Width) //need to switch to another line
                {
                    ArrangeLine(accumulatedHeight, curLineSize, arrangeBounds.Width, firstInLine, i);

                    accumulatedHeight += curLineSize.Height;
                    curLineSize = sz;

                    if (sz.Width > arrangeBounds.Width) //the element is wider then the constraint - give it a separate line
                    {
                        ArrangeLine(accumulatedHeight, sz, arrangeBounds.Width, i, ++i);
                        accumulatedHeight += sz.Height;
                        curLineSize = new Size();
                    }
                    firstInLine = i;
                }
                else //continue to accumulate a line
                    curLineSize = ContinueLine(curLineSize, sz);
            }

            if (firstInLine < children.Count)
                ArrangeLine(accumulatedHeight, curLineSize, arrangeBounds.Width, firstInLine, children.Count);

            return arrangeBounds;
        }

        private static Size AddNewLine(Size curLineSize, Size panelSize)
        {
            panelSize.Width = Math.Max(curLineSize.Width, panelSize.Width);
            panelSize.Height += curLineSize.Height;
            return panelSize;
        }

        private static Size ContinueLine(Size curLineSize, Size sz)
        {
            curLineSize.Width += sz.Width;
            curLineSize.Height = Math.Max(sz.Height, curLineSize.Height);
            return curLineSize;
        }

        private void ArrangeLine(double y, Size lineSize, double boundsWidth, int start, int end)
        {
            UIElementCollection children = InternalChildren;

            Dictionary<HorizontalAlignment, List<UIElement>> controls = new()
            {
                { HorizontalAlignment.Left, new () },
                { HorizontalAlignment.Center, new () },
                { HorizontalAlignment.Right, new () }
            };

            // sort line contents by alignment
            for (int i = start; i < end; i++)
            {
                UIElement child = children[i];
                HorizontalAlignment alignment = HorizontalContentAlignment;

                if (child is FrameworkElement element)
                    alignment = element.HorizontalAlignment;

                // check
                if (alignment == HorizontalAlignment.Stretch)
                    throw new InvalidOperationException(HorizontalAlignment.Stretch + " horizontal alignment isn't supported.");

                // put element into the hash
                List<UIElement> list = controls[alignment];

                list.Add(child);
            }

            // calculate center gap size
            double centerGap = (boundsWidth - lineSize.Width) / 2;

            double x = 0.0;
            foreach (HorizontalAlignment alignment in new[] { HorizontalAlignment.Left, HorizontalAlignment.Center, HorizontalAlignment.Right })
            {
                // get element list
                List<UIElement> list = controls[alignment];

                // arrange all elements
                foreach (UIElement child in list)
                {
                    child.Arrange(new Rect(x, y, child.DesiredSize.Width, lineSize.Height));
                    x += child.DesiredSize.Width;
                }

                // move a center gap
                x += centerGap;
            }
        }
    }
}