//--------------------------------------------------------------------------
// <copyright file="EditExclusionDialog.cs" company="Jeff Winn">
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

namespace Winnster.CheckInPolicies.SourceAnalysis.UI.Forms
{
    using System;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;
    using Winnster.CheckInPolicies.SourceAnalysis.Policy.Exclusions;
    using Winnster.CheckInPolicies.SourceAnalysis.UI.Controls;
    using Winnster.CheckInPolicies.SourceAnalysis.UI.Forms.Design;
    using Winnster.CheckInPolicies.SourceAnalysis.UI.Forms.Editors;
    using Winnster.CheckInPolicies.SourceAnalysis.UI.Forms.Editors.Design;

    /// <summary>
    /// Provides a user interface for editing exclusions. This class cannot be inherited.
    /// </summary>
    internal sealed partial class EditExclusionDialog : BaseDialog
    {
        #region Fields

        /// <summary>
        /// Holds the value of the <see cref="Exclusions"/> property.
        /// </summary>
        private Collection<IPolicyExclusion> exclusions;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Winnster.CheckInPolicies.SourceAnalysis.UI.Forms.EditExclusionDialog"/> class.
        /// </summary>
        public EditExclusionDialog()
        {
            this.InitializeComponent();
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the user interface editor type.
        /// </summary>
        [Browsable(false)]
        public Type EditorType
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the edit mode.
        /// </summary>
        [Browsable(false)]
        public EditorMode EditMode
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the collection of exclusions to modify.
        /// </summary>
        [Browsable(false)]
        public Collection<IPolicyExclusion> Exclusions
        {
            get
            {
                if (this.exclusions == null)
                {
                    this.exclusions = new Collection<IPolicyExclusion>();
                }

                return this.exclusions;
            }

            set
            {
                this.exclusions = value;
            }
        }

        #endregion

        /// <summary>
        /// Validates the form.
        /// </summary>
        /// <returns><b>true</b> if the form is valid; otherwise, <b>false</b>.</returns>
        protected override bool ValidateForm()
        {
            return base.ValidateForm() && this.ExclusionsControl.Items.Count > 0;
        }

        /// <summary>
        /// Disables a <see cref="ListViewItem"/> instance.
        /// </summary>
        /// <param name="item">The <see cref="ListViewItem"/> to disable.</param>
        /// <exception cref="System.ArgumentNullException"><paramref name="item"/> is a null reference (<c>Nothing</c> in Visual Basic).</exception>
        private static void DisableListViewItem(ListViewItem item)
        {
            if (item == null)
            {
                throw new ArgumentNullException("item");
            }

            item.Font = new Font(item.Font, FontStyle.Italic);
        }

        /// <summary>
        /// Enables a <see cref="ListViewItem"/> instance.
        /// </summary>
        /// <param name="item">The <see cref="ListViewItem"/> to enable.</param>
        /// <exception cref="System.ArgumentNullException"><paramref name="item"/> is a null reference (<c>Nothing</c> in Visual Basic).</exception>
        private static void EnableListViewItem(ListViewItem item)
        {
            if (item == null)
            {
                throw new ArgumentNullException("item");
            }

            item.Font = new Font(item.Font, FontStyle.Regular);
        }

        /// <summary>
        /// Creates a new editor dialog.
        /// </summary>
        /// <returns>A new <see cref="BaseEditorDialog"/> instance.</returns>
        private BaseEditorDialog CreateEditorDialog()
        {
            return (BaseEditorDialog)Activator.CreateInstance(this.EditorType);
        }

        /// <summary>
        /// Occurs when the <see cref="ExclusionsControl"/> is adding an item.
        /// </summary>
        /// <param name="sender">The <see cref="System.Object"/> that raised the event.</param>
        /// <param name="e">An <see cref="AddingItemEventArgs"/> containing event data.</param>
        private void ExclusionsControl_AddingItem(object sender, ItemEventArgs e)
        {
            using (BaseEditorDialog dialog = this.CreateEditorDialog())
            {
                dialog.EditMode = EditorMode.Add;

                if (dialog.ShowDialog(this) != DialogResult.OK)
                {
                    e.Cancel = true;
                }
                else
                {
                    IPolicyExclusion newExclusion = dialog.Value;
                    newExclusion.Active = true;

                    bool found = false;
                    foreach (IPolicyExclusion exclusion in this.Exclusions)
                    {
                        if (exclusion.CompareTo(newExclusion) == 0)
                        {
                            // The new exclusion already exists, notify the user and do not add it again.
                            found = true;
                            break;
                        }
                    }

                    if (!found)
                    {
                        this.Exclusions.Add(newExclusion);

                        e.Item.Text = newExclusion.Description;
                        e.Item.Tag = newExclusion;
                    }

                    this.EnableSubmitButton();
                }
            }
        }

        /// <summary>
        /// Occurs when the <see cref="ExclusionsControl"/> has added an item.
        /// </summary>
        /// <param name="sender">The <see cref="System.Object"/> that raised the event.</param>
        /// <param name="e">An <see cref="System.EventArgs"/> containing event data.</param>
        private void ExclusionsControl_AddedItem(object sender, EventArgs e)
        {
            this.ExclusionsControl.ListViewTabStop = true;
            this.EnableSubmitButton();
        }

        /// <summary>
        /// Occurs when the <see cref="ExclusionsControl"/> is removing an item.
        /// </summary>
        /// <param name="sender">The <see cref="System.Object"/> that raised the event.</param>
        /// <param name="e">An <see cref="System.EventArgs"/> containing event data.</param>
        private void ExclusionsControl_RemovingItem(object sender, ItemEventArgs e)
        {
            IPolicyExclusion exclusion = (IPolicyExclusion)e.Item.Tag;

            int removeIndex = int.MinValue;

            for (int index = 0; index < this.Exclusions.Count; index++)
            {
                if (exclusion == this.Exclusions[index])
                {
                    removeIndex = index;
                    break;
                }
            }

            if (removeIndex != int.MinValue)
            {
                this.Exclusions.RemoveAt(removeIndex);
            }
        }

        /// <summary>
        /// Occurs when the <see cref="ExclusionsControl"/> has removed an item.
        /// </summary>
        /// <param name="sender">The <see cref="System.Object"/> that raised the event.</param>
        /// <param name="e">An <see cref="System.EventArgs"/> containing event data.</param>
        private void ExclusionsControl_RemovedItem(object sender, EventArgs e)
        {
            this.ExclusionsControl.ListViewTabStop = this.ExclusionsControl.Items.Count > 0;

            this.EnableSubmitButton();
        }

        /// <summary>
        /// Occurs when the <see cref="ExclusionsControl"/> is editing an item.
        /// </summary>
        /// <param name="sender">The <see cref="System.Object"/> that raised the event.</param>
        /// <param name="e">An <see cref="System.EventArgs"/> containing event data.</param>
        private void ExclusionsControl_EditingItem(object sender, ItemEventArgs e)
        {
            IPolicyExclusion policy = (IPolicyExclusion)e.Item.Tag;

            using (BaseEditorDialog dialog = this.CreateEditorDialog())
            {
                dialog.EditMode = EditorMode.Edit;
                dialog.Value = policy;

                if (dialog.ShowDialog(this) != DialogResult.OK)
                {
                    e.Cancel = true;
                }
                else
                {
                    // Update the policy description.
                    e.Item.Text = policy.Description;
                }
            }
        }

        /// <summary>
        /// Occurs when the <see cref="ManageExclusionDialog"/> is loading.
        /// </summary>
        /// <param name="sender">The <see cref="System.Object"/> that raised the event.</param>
        /// <param name="e">An <see cref="System.EventArgs"/> containing event data.</param>
        private void ManageExclusionDialog_Load(object sender, EventArgs e)
        {
            if (this.EditMode == EditorMode.Edit)
            {
                if (this.Exclusions != null && this.Exclusions.Count == 0)
                {
                    this.ExclusionsControl.ListViewTabStop = false;
                }
                else
                {
                    this.ExclusionsControl.ListViewTabStop = true;

                    foreach (IPolicyExclusion exclusion in this.Exclusions)
                    {
                        ListViewItem item = new ListViewItem();
                        item.Text = exclusion.Description;
                        item.Tag = exclusion;

                        if (!exclusion.Active)
                        {
                            DisableListViewItem(item);
                        }

                        this.ExclusionsControl.Items.Add(item);
                    }

                    this.EnableSubmitButton();
                }
            }
        }

        /// <summary>
        /// Occurs when the <see cref="ExclusionsControl"/> index has changed.
        /// </summary>
        /// <param name="sender">The <see cref="System.Object"/> that raised the event.</param>
        /// <param name="e">An <see cref="EnableButtonsEventArgs"/> containing event data.</param>
        private void ExclusionsControl_SelectedIndexChanged(object sender, EnableButtonsEventArgs e)
        {
            IPolicyExclusion exclusion = (IPolicyExclusion)e.Item.Tag;

            if (exclusion != null)
            {
                e.EnableButton = !exclusion.Active;
                e.DisableButton = exclusion.Active;
            }
        }

        /// <summary>
        /// Occurs when the <see cref="ExclusionsControl"/> is disabling an item.
        /// </summary>
        /// <param name="sender">The <see cref="System.Object"/> that raised the event.</param>
        /// <param name="e">An <see cref="ItemEventArgs"/> containing event data.</param>
        private void ExclusionsControl_DisablingItem(object sender, ItemEventArgs e)
        {
            IPolicyExclusion exclusion = (IPolicyExclusion)e.Item.Tag;

            if (exclusion != null)
            {
                exclusion.Active = false;

                DisableListViewItem(e.Item);
            }
        }

        /// <summary>
        /// Occurs when the <see cref="ExclusionsControl"/> is enabling an item.
        /// </summary>
        /// <param name="sender">The <see cref="System.Object"/> that raised the event.</param>
        /// <param name="e">An <see cref="ItemEventArgs"/> containing event data.</param>
        private void ExclusionsControl_EnablingItem(object sender, ItemEventArgs e)
        {
            IPolicyExclusion exclusion = (IPolicyExclusion)e.Item.Tag;

            if (exclusion != null)
            {
                exclusion.Active = true;

                EnableListViewItem(e.Item);
            }
        }
    }
}