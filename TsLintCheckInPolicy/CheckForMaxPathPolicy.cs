// --------------------------------------------------------------------------------------------------------------------
// <copyright company="Luxoft Company" file="CheckForMaxPathPolicy.cs">
//   Copyright (C) 2015 Luxoft Company. All Rights Reserved
// </copyright>
// 
// --------------------------------------------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Windows.Forms;

using Microsoft.TeamFoundation.VersionControl.Client;

namespace FileEncodingCheckInPolicy
{
    [Serializable]
    public class CheckForMaxPathPolicy : PolicyBase
    {
        private const int USER_MAX_PATH = 220;

        //registry location where policy defined max path is stored
        public static int defaultMaxPath = 220;

        public override string Description
        {
            get { return "Detects if check in will approach TFS version control MAX PATH of 260 characters.  The threshold for this detection is configurable."; }

        }

        // This is a string that is stored with the policy definition on the source
        // control server. If a user does not have the policy plug-in installed, this string
        // is displayed.  You can use this to explain to the user how they should 
        // install the policy plug-in.
        public override string InstallationInstructions
        {
            get { return "To install this policy, read \\\\[your server]\\[your share]\\InstallInstructions.txt or contact your local build contact."; }
        }

        // This string identifies the type of policy. It is displayed in the 
        // policy list when you add a new policy to a Team Project.
        public override bool Edit(IPolicyEditArgs policyEditArgs)
        {
            return false;
        }

        public override string Type
        {
            get { return "MAX PATH detection"; }
        }

        // This string is a description of the type of policy. It is displayed 
        // when you select the policy in the Add Check-in Policy dialog box.
        public override string TypeDescription
        {
            get { return "This policy will detect if a check in will approach TFS version control MAX PATH of 260 characters."; }
        }


        // This method performs the actual policy evaluation. 
        // It is called by the policy framework at various points in time
        // when policy should be evaluated. In this example, the method 
        // is invoked when various asyc events occur that may have 
        // invalidated the current list of failures.
        public override PolicyFailure[] Evaluate()
        {

            List<PolicyFailure> failures = new List<PolicyFailure>();

            foreach (PendingChange change in PendingCheckin.PendingChanges.AllPendingChanges)
            {
                if (change.ServerItem.ToString().Length >= USER_MAX_PATH)
                {
                    failures.Add(new PolicyFailure(
                        change.FileName.ToString() + " path length of " + change.ServerItem.ToString().Length.ToString() + " exceeds MAX PATH threshold of " + USER_MAX_PATH + ".  Full server path is " + change.ServerItem.ToString(), this));
                }
            }
            return failures.ToArray();
        }

        // This method is called if the user double-clicks on 
        // a policy failure in the UI. In this case a message telling the user 
        // to supply some comments is displayed.
        public override void Activate(PolicyFailure failure)
        {
            MessageBox.Show("Please reduce the file or directory name length to less than 260 characters", "How to fix your policy failure");
        }

        // This method is called if the user presses F1 when a policy failure 
        // is active in the UI. In this example, a message box is displayed.
        public override void DisplayHelp(PolicyFailure failure)
        {
            MessageBox.Show("This policy detects if your check in is approaching the TFS Version control MAX PATH of 260 characters and and reminds you to reduce the file and directory length.", "Policy Help");
        }
    }
}