﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace MusicBeePlugin.Settings
{
    class ApplicationSettings
    {
        #region properties
        public int ListeningPort { get; set; }

        public FilteringSelection FilterSelection { get; set; }

        public string BaseIp { get; set; }

        public int LastOctetMax { get; set; }

        public List<string> IpAddressList { get; set; }
        #endregion

        #region methods
        public string FlattenAllowedAddressList()
        {
            if (IpAddressList == null)
            {
                IpAddressList = new List<string>();
            }
            return IpAddressList.Aggregate<string, string>(
                null,
                (current, s) => current + (s + ",")
            );
        }

        public void UpdateFilteringSelection(string selection)
        {
            switch (selection)
            {
                case "All":
                    FilterSelection = FilteringSelection.All;
                    break;
                case "Range":
                    FilterSelection = FilteringSelection.Range;
                    break;
                case "Specific":
                    FilterSelection = FilteringSelection.Specific;
                    break;
            }
        }

        public void UnflattenAllowedAddressList(string allowedAddresses)
        {
            IpAddressList = new List<string>(allowedAddresses.Trim().Split(
                ",".ToCharArray(),
                StringSplitOptions.RemoveEmptyEntries
            ));
        }
        #endregion
    }
}
