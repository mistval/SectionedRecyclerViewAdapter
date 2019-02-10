/*
 * MIT License

* Copyright (c) 2017 mistval

* Permission is hereby granted, free of charge, to any person obtaining a copy
* of this software and associated documentation files (the "Software"), to deal
* in the Software without restriction, including without limitation the rights
* to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
* copies of the Software, and to permit persons to whom the Software is
* furnished to do so, subject to the following conditions:

* The above copyright notice and this permission notice shall be included in all
* copies or substantial portions of the Software.

* THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
* IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
* FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
* AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
* LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
* OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
* SOFTWARE. */

using Android.Support.V7.Widget;
using Android.Views;

namespace Controls
{
    public abstract class SectionedRecyclerViewAdapter : RecyclerView.Adapter
    {
        #region Constants

        private const int SECTION_HEADER_VIEW_TYPE = 0;
        private const int ITEM_VIEW_TYPE = 1;

        #endregion

        #region Properties

        public sealed override int ItemCount
        {
            get
            {
                var sectionCount = this.SectionCount;
                var itemCount = 0;
                for (var i = 0; i < sectionCount; ++i)
                {
                    itemCount += this.GetItemCountForSection(i);
                }

                return itemCount + sectionCount;
            }
        }

        #endregion

        #region Properties

        protected abstract int SectionCount { get; }

        #endregion

        #region Methods

        public sealed override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            if (this.IsSectionHeader(position))
            {
                this.BindSectionHeaderViewHolder(holder, this.GetSection(position));
            }
            else
            {
                this.BindItemViewHolder(holder, this.GetSection(position), this.GetItem(position));
            }
        }

        public sealed override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            if (viewType == SectionedRecyclerViewAdapter.SECTION_HEADER_VIEW_TYPE)
            {
                return this.CreateSectionHeaderViewHolder(parent);
            }
            else
            {
                return this.CreateItemViewHolder(parent);
            }
        }

        public sealed override int GetItemViewType(int position)
        {
            return this.IsSectionHeader(position)
                ? SectionedRecyclerViewAdapter.SECTION_HEADER_VIEW_TYPE
                : SectionedRecyclerViewAdapter.ITEM_VIEW_TYPE;
        }

        public void NotifyItemInserted(int section, int item)
        {
            this.NotifyItemInserted(this.GetPosition(section, item));
        }

        public void NotifyItemChanged(int section, int item)
        {
            this.NotifyItemChanged(this.GetPosition(section, item));
        }

        public void NotifyItemMoved(int fromSection, int fromItem, int toSection, int toItem)
        {
            this.NotifyItemMoved(this.GetPosition(fromSection, fromItem), this.GetPosition(toSection, toItem));
        }

        public void NotifyItemRemoved(int section, int item)
        {
            this.NotifyItemRemoved(this.GetPosition(section, item));
        }

        public int GetPosition(int section, int item)
        {
            var position = 1;

            for (var i = 0; i < section; ++i)
            {
                position += this.GetItemCountForSection(i) + 1;
            }

            position += item;
            return position;
        }

        protected abstract int GetItemCountForSection(int section);

        protected abstract void BindSectionHeaderViewHolder(RecyclerView.ViewHolder holder, int section);

        protected abstract void BindItemViewHolder(RecyclerView.ViewHolder holder, int section, int item);

        protected abstract RecyclerView.ViewHolder CreateSectionHeaderViewHolder(ViewGroup parent);

        protected abstract RecyclerView.ViewHolder CreateItemViewHolder(ViewGroup parent);

        private bool IsSectionHeader(int position)
        {
            var nextSectionHeaderIndex = 0;
            var sections = this.SectionCount;

            for (var i = 0; i < sections; i += 1)
            {
                if (nextSectionHeaderIndex == position)
                {
                    return true;
                }

                nextSectionHeaderIndex += this.GetItemCountForSection(i) + 1;
            }

            return false;
        }

        private int GetSection(int position)
        {
            var nextSectionStartPositionIndex = 0;
            var sections = this.SectionCount;

            for (var i = 0; i < sections; i += 1)
            {
                if (nextSectionStartPositionIndex > position)
                {
                    return i - 1;
                }

                nextSectionStartPositionIndex += this.GetItemCountForSection(i) + 1;
            }

            return sections - 1;
        }

        private int GetItem(int position)
        {
            var currentSectionStartPosition = 0;
            var sections = this.SectionCount;

            for (var i = 0; i < sections; i += 1)
            {
                var currentSectionItemCount = this.GetItemCountForSection(i);
                var nextSectionStartPosition = currentSectionStartPosition + currentSectionItemCount + 1;

                if (nextSectionStartPosition > position)
                {
                    return position - currentSectionStartPosition - 1;
                }

                currentSectionStartPosition = nextSectionStartPosition;
            }

            return position - currentSectionStartPosition - 1;
        }

        #endregion
    }
}