// GlobalSettings.cs
//
// Copyright 2011 Phillip Stephens
//
// This program is free software; you can redistribute it and/or
// modify it under the terms of the GNU General Public License
// as published by the Free Software Foundation; either version 2
// of the License, or (at your option) any later version.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program; if not, write to the Free Software
// Foundation, Inc., 59 Temple Place - Suite 330, Boston, MA  02111-1307, USA.
//
// Linking this library statically or dynamically with other modules is
// making a combined work based on this library.  Thus, the terms and
// conditions of the GNU General Public License cover the whole
// combination.
// 
// As a special exception, the copyright holders of this library give you
// permission to link this library with independent modules to produce an
// executable, regardless of the license terms of these independent
// modules, and to copy and distribute the resulting executable under
// terms of your choice, provided that you also meet, for each linked
// independent module, the terms and conditions of the license of that
// module.  An independent module is a module which is not derived from
// or based on this library.  If you modify this library, you may extend
// this exception to your version of the library, but you are not
// obligated to do so.  If you do not wish to do so, delete this
// exception statement from your version.

using System;
using System.Collections.Generic;

namespace GameMaker.ProjectCommon
{
    public class GlobalSettings
    {


        private int m_gameId;
        private byte[] m_guid;
        private bool m_fullscreen;
        private bool m_interpolate;
        private bool m_noBorders;
        private bool m_displayCursor;
        private int m_scaling;
        private bool m_allowResizing;
        private bool m_stayOnTop;
        private int m_outsideColor;
        private bool m_setResolution;
        private ColorDepth1 m_colorDepth1;
        private bool m_useExclusiveGraphicsMode;
        private Resolution1 m_resolution1;
        private Frequency1 m_frequency1;
        private ColorDepth2 m_colorDepth2;
        private Resolution2 m_resolution2;
        private Frequency2 m_frequency2;
        private bool m_noButtons;
        private bool m_useVSync;
        private bool m_disableScreenSavers;
        private bool m_f4ChangeScreenMode;
        private bool m_f1ShowInformation;
        private bool m_escapeEndGame;
        private bool m_f5SaveF6Load;
        private bool m_f9ScreenShot;
        private bool m_closeButtonAsEsc;
        private Priority m_priority;
        private bool m_freezeOnLoseFocus;
        private ProgressBar m_progressBar;
        private byte[] m_icon;
        private bool m_displayErrors;
        private bool m_writeLog;
        private bool m_abortOnErrors;
        private bool m_treatUninitializedAsZero;
        private string m_author;
        private string m_version;
        private DateTime m_settingsLastModified;
        private string m_information;
        private int m_major;
        private int m_minor;
        private int m_release;
        private int m_build;
        private string m_company;
        private string m_product;
        private string m_copyright;
        private string m_description;
        private DateTime m_buildLastModified;
        private List<Constant> m_constants = new List<Constant>();


        public int GameId
        {
            get { return m_gameId; }
            set { m_gameId = value; }
        }

        public byte[] Guid
        {
            get { return m_guid; }
            set { m_guid = value; }
        }

        public bool Fullscreen
        {
            get {return m_fullscreen; }
            set {m_fullscreen = value;}
        }
        public bool Interpolate
        {
            get {return m_interpolate; }
            set {m_interpolate = value;}
        }
        public bool NoBorders
        {
            get { return m_noBorders; }
            set { m_noBorders = value; }
        }

        public bool DisplayCursor
        {
            get { return m_displayCursor; }
            set { m_displayCursor = value; }
        }

        public int Scaling
        {
            get { return m_scaling; }
            set { m_scaling = value; }
        }

        public bool AllowResizing
        {
            get { return m_allowResizing; }
            set { m_allowResizing = value; }
        }

        public bool StayOnTop
        {
            get { return m_stayOnTop; }
            set { m_stayOnTop = value; }
        }

        public int OutsideColor
        {
            get { return m_outsideColor; }
            set { m_outsideColor = value; }
        }

        public bool SetResolution
        {
            get { return m_setResolution; }
            set { m_setResolution = value; }
        }

        public ColorDepth1 ColorDepth1
        {
            get { return m_colorDepth1; }
            set { m_colorDepth1 = value; }
        }

        public bool UseExclusiveGraphicsMode
        {
            get { return m_useExclusiveGraphicsMode; }
            set { m_useExclusiveGraphicsMode = value; }
        }

        public Resolution1 Resolution1
        {
            get { return m_resolution1; }
            set { m_resolution1 = value; }
        }

        public Frequency1 Frequency1
        {
            get { return m_frequency1; }
            set { m_frequency1 = value; }
        }

        public ColorDepth2 ColorDepth2
        {
            get { return m_colorDepth2; }
            set { m_colorDepth2 = value; }
        }

        public Resolution2 Resolution2
        {
            get { return m_resolution2; }
            set { m_resolution2 = value; }
        }

        public Frequency2 Frequency2
        {
            get { return m_frequency2; }
            set { m_frequency2 = value; }
        }

        public bool NoButtons
        {
            get { return m_noButtons; }
            set { m_noButtons = value; }
        }

        public bool UseVSync
        {
            get { return m_useVSync; }
            set { m_useVSync = value; }
        }

        public bool DisableScreensavers
        {
            get { return m_disableScreenSavers; }
            set { m_disableScreenSavers = value; }
        }

        public bool F4ChangeScreenModes
        {
            get { return m_f4ChangeScreenMode; }
            set { m_f4ChangeScreenMode = value; }
        }

        public bool F1ShowInformation
        {
            get { return m_f1ShowInformation; }
            set { m_f1ShowInformation = value; }
        }

        public bool EscapeEndGame
        {
            get { return m_escapeEndGame; }
            set { m_escapeEndGame = value; }
        }

        public bool F5SaveF6Load
        {
            get { return m_f5SaveF6Load; }
            set { m_f5SaveF6Load = value; }
        }

        public bool F9Screenshot
        {
            get { return m_f9ScreenShot; }
            set { m_f9ScreenShot = value; }
        }

        public bool CloseButtonAsEsc
        {
            get { return m_closeButtonAsEsc; }
            set { m_closeButtonAsEsc = value; }
        }

        public Priority Priority
        {
            get { return m_priority; }
            set { m_priority = value; }
        }

        public bool FreezeOnLoseFocus
        {
            get { return m_freezeOnLoseFocus; }
            set { m_freezeOnLoseFocus = value; }
        }

        public ProgressBar ProgressBar
        {
            get { return m_progressBar; }
            set { m_progressBar = value; }
        }

        public byte[] Icon
        {
            get { return m_icon; }
            set { m_icon = value; }
        }

        public bool DisplayErrors
        {
            get { return m_displayErrors; }
            set { m_displayErrors = value; }
        }

        public bool WriteLog
        {
            get { return m_writeLog; }
            set { m_writeLog = value; }
        }

        public bool AbortOnErrors
        {
            get { return m_abortOnErrors; }
            set { m_abortOnErrors = value; }
        }

        public bool TreatUninitializedAsZero
        {
            get { return m_treatUninitializedAsZero; }
            set { m_treatUninitializedAsZero = value; }
        }

        public string Author
        {
            get { return m_author; }
            set { m_author = value; }
        }

        public string Version
        {
            get { return m_version; }
            set { m_version = value; }
        }

        public DateTime SettingsLastModified
        {
            get { return m_settingsLastModified; }
            set { m_settingsLastModified = value; }
        }

        public string Information
        {
            get { return m_information; }
            set { m_information = value; }
        }

        public int Major
        {
            get { return m_major; }
            set { m_major = value; }
        }

        public int Minor
        {
            get { return m_minor; }
            set { m_minor = value; }
        }

        public int Release
        {
            get { return m_release; }
            set { m_release = value; }
        }

        public int Build
        {
            get { return m_build; }
            set { m_build = value; }
        }

        public string Company
        {
            get { return m_company; }
            set { m_company = value; }
        }

        public string Product
        {
            get { return m_product; }
            set { m_product = value; }
        }

        public string Copyright
        {
            get { return m_copyright; }
            set { m_copyright = value; }
        }

        public string Description
        {
            get { return m_description; }
            set { m_description = value; }
        }

        public DateTime BuildLastModified
        {
            get { return m_buildLastModified; }
            set { m_buildLastModified = value; }
        }

        public List<Constant> Constants
        {
            get { return m_constants; }
            internal set { m_constants = value; }
        }


    }

    public class Constant
    {
        private static DateTime lastChanged = DateTime.Now;
        private string m_name;
        private string m_value;
        private bool m_hidden;

        public static DateTime LastChanged
        {
            get { return lastChanged; }
            set { lastChanged = value; }
        }

        public string Name
        {
            get { return m_name; }
            set { m_name = value; }
        }

        public string Value
        {
            get { return m_value; }
            set { m_value = value; }
        }

        public bool Hidden
        {
            get { return m_hidden; }
            set { m_hidden = value; }
        }
    }

    public class Include
    {


        private DateTime m_lastChanged = DateTime.Now;
        private string m_filename = string.Empty;
        private string m_filepath = string.Empty;
        private bool m_originalFile;
        private int m_originalFileSize;
        private bool m_storeInEditable;
        private byte[] m_fileData;
        private string m_exportFolder = string.Empty;
        private Export m_exportMode = Export.WorkingFolder;
        private bool m_overwrite;
        private bool m_freeMemory;
        private bool m_removeAtEnd;



        public DateTime LastChanged
        {
            get { return m_lastChanged; }
            set { m_lastChanged = value; }
        }

        public string Filename
        {
            get { return m_filename; }
            set { m_filename = value; }
        }

        public string FilePath
        {
            get { return m_filepath; }
            set { m_filepath = value; }
        }

        public bool OriginalFile
        {
            get { return m_originalFile; }
            set { m_originalFile = value; }
        }

        public int OriginalFileSize
        {
            get { return m_originalFileSize; }
            set { m_originalFileSize = value; }
        }

        public bool StoreInEditable
        {
            get { return m_storeInEditable; }
            set { m_storeInEditable = value; }
        }

        public byte[] FileData
        {
            get { return m_fileData; }
            set { m_fileData = value; }
        }

       public string ExportFolder
        {
            get {return m_exportFolder;}
            set {m_exportFolder = value;}
        }

        public Export ExportMode
        {
            get { return m_exportMode; }
            set { m_exportMode = value; }
        }


        public bool Overwrite
        {
            get { return m_overwrite; }
            set { m_overwrite = value; }
        }

        public bool FreeMemory
        {
            get { return m_freeMemory; }
            set { m_freeMemory = value; }
        }

        public bool RemoveAtEnd
        {
            get { return m_removeAtEnd; }
            set { m_removeAtEnd = value; }
        }


    }
}
