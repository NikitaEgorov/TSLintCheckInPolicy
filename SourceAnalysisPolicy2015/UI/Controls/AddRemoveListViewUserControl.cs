//--------------------------------------------------------------------------
// <copyright file="AddRemoveListViewUserControl.cs" company="Jeff Winn">
//      Copyright (c) Jeff Winn. All rights reserved.
//
//      The use and distribution terms for this software is covered by the
//      Microsoft Public License (Ms-PL) which can be found in the License.rtf 
//      at the root of this distribution.
//      By using this software in any fashion, you are agreeing to be bound by
//      the terms of this license.
//
//      You must not remove this notice, or any other, from this software.
// </copyright>
//--------------------------------------------------------------------------

namespace Winnster.CheckInPolicies.SourceAnalysis.UI.Controls
{
    using System;
    using System.ComponentModel;
    using System.Globalization;
    using System.Windows.Forms;

    /// <summary>
    /// Provides a <see cref="ListView"/> control with add/remove buttons. This class cannot be inherited.
    /// </summary>
    internal sealed partial class AddRemoveListViewUserControl : UserControl
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Winnster.CheckInPolicies.SourceAnalysis.UI.Controls.AddRemoveListViewUserControl"/> class.
        /// </summary>
        public AddRemoveListViewUserControl()
        {
            this.InitializeComponent();
        }

        #endregion

        #region Events

        /// <summary>
        /// Occurs when an item is being added.
        /// </summary>
        public event EventHandler<CancelEventArgs> AddingItem;

        /// <summary>
        /// Occurs after an item has been added.
        /// </summary>
        public event EventHandler<EventArgs> AddedItem;

        /// <summary>
        /// Occurs when an item has been removed.
        /// </summary>
        public event EventHandler<EventArgs> RemovedItem;

        #endregion

        #region Properties

        /// <summary>
        /// Gets the collection of items on the control.
        /// </summary>
        public ListView.ListViewItemCollection Items
        {
            get
            {
                return this.DataListView.Items;
            }
        }

        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        [DefaultValue(null)]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "Needed for designer support.")]
        public string Title
        {
            get
            {
                return this.TitleLabel.Text;
            }

            set
            {
                this.TitleLabel.Text = value;
            }
        }

        #endregion

        /// <summary>
        /// Raises the <see cref="AddingItem"/> event.
        /// </summary>
        /// <param name="e">An <see cref="System.EventArgs"/> containing event data.</param>
        private void OnAddingItem(CancelEventArgs e)
        {
            if (this.AddingItem != null)
            {
                this.AddingItem(this, e);
            }
        }

        /// <summary>
        /// Raises the <see cref="AddedItem"/> event.
        /// </summary>
        /// <param name="e">An <see cref="System.EventArgs"/> containing event data.</param>
        private void OnAddedItem(EventArgs e)
        {
            if (this.AddedItem != null)
            {
                this.AddedItem(this, e);
            }
        }

        /// <summary>
        /// Raises the <see cref="RemovedItem"/> event.
        /// </summary>
        /// <param name="e">An <see cref="System.EventArgs"/> containing event data.</param>
        private void OnRemovedItem(EventArgs e)
        {
            if (this.RemovedItem != null)
            {
                this.RemovedItem(this, e);
            }
        }

        /// <summary>
        /// Adds an item to the <see cref="ListView"/> control.
        /// </summary>
        private void AddItem()
        {
            string value = null;

            using (AddEntryDialog dialog = new AddEntryDialog())
            {
                if (dialog.ShowDialog(this) == DialogResult.OK)
                {
                    value = dialog.Value;
                }
            }

            if (!string.IsNullOrEmpty(value))
            {
                bool found = false;

                foreach (ListViewItem item in this.DataListView.Items)
                {
                    if (string.Compare(item.Text, value, true, CultureInfo.CurrentCulture) == 0)
                    {
                        found = true;
                    }
                }

                if (!found)
                {
                    CancelEventArgs e = new CancelEventArgs();
                    this.OnAddingItem(e);

                    if (!e.Cancel)
                    {
                        ListViewItem item = new ListViewItem(value);
                        item.Checked = true;

                        this.DataListView.Items.Add(item);

                        this.OnAddedItem(EventArgs.Empty);
                    }
                }
            }
        }

        /// <summary>
        /// Removes an item from the <see cref="ListView"/> control.
        /// </summary>
        private void RemoveItem()
        {
            while (this.DataListView.SelectedItems.Count > 0)
            {
                ListViewItem item = this.DataListView.SelectedItems[0];
                if (item != null)
                {
                    item.Remove();

                    this.OnRemovedItem(EventArgs.Empty);
                }
            }
        }

        /// <summary>
        /// Occurs when the <see cref="AddButton"/> is clicked.
        /// </summary>
        /// <param name="sender">The <see cref="System.Object"/> that raised the event.</param>
        /// <param name="e">An <see cref="System.EventArgs"/> containing event data.</param>
        private void AddButton_Click(object sender, EventArgs e)
        {
            this.AddItem();
        }

        /// <summary>
        /// Occurs when the <see cref="RemoveButton"/> is clicked.
        /// </summary>
        /// <param name="sender">The <see cref="System.Object"/> that raised the event.</param>
        /// <param name="e">An <see cref="System.EventArgs"/> containing event data.</param>
        private void RemoveButton_Click(object sender, EventArgs e)
        {
            this.RemoveItem();
        }

        /// <summary>
        /// Occurs when the <see cref="PathListView"/> selected index has changed.
        /// </summary>
        /// <param name="sender">The <see cref="System.Object"/> that raised the event.</param>
        /// <param name="e">An <see cref="System.EventArgs"/> containing event data.</param>
        private void PathListView_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.RemoveButton.Enabled = this.DataListView.SelectedItems.Count > 0;
        }
    }
}