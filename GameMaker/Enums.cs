// Enums.cs
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

namespace GameMaker
{
    public enum ActionEnd
    {
        StopMoving,
        JumpToStart,
        MoveToStart,
        Reverse,
        Continue
    };

    public enum ActionKind
    {
        Normal,
        BeginGroup,
        EndGroup,
        Else,
        Exit,
        Repeat,
        Variable,
        Code,
        PlaceHolder,
        Separator,
        Label
    };

    public enum ArgumentType
    {
        Expression,
        String,
        Both,
        Boolean,
        Menu,
        Sprite,
        Sound,
        Background,
        Path,
        Object,
        Room,
        Font,
        Color,
        Timeline,
        FontString
    };

    public enum BoundingBox
    {
        Auto,
        FullImage,
        Manual,
    };

    public enum CallConvention
    {
        GML = 2,
        STDCALL = 11,
        CDECL
    };

    public enum ColorDepth1
    {
        Color16Bit,
        Color32Bit
    };

    public enum ColorDepth2
    {
        None,
        Color16Bit,
        Color32Bit
    };

    public enum EventType
    {
        None = -1,
        Create,
        Destroy,
        Alarm,
        Step,
        Collision,
        Keyboard,
        Mouse,
        Other,
        Draw,
        KeyPress,
        KeyRelease,
        Trigger
    };

    public enum ExecutionType
    {
        None,
        Function,
        Code
    };

    public enum Export
    {
        Dont,
        Temp,
        WorkingFolder,
        Defined
    };

    public enum FileType
    {
        Dll = 1,
        Gml,
        Library,
        Other
    };

    public enum FileVersion
    {
        GameMaker50 = 500,
        GameMaker51 = 510,
        GameMaker52 = 520,
        GameMaker53 = 520,
        GameMaker60 = 600,
        GameMaker70 = 701,
        GameMaker80 = 800,
        GameMaker81 = 810
    };

    public enum Frequency1
    {
        Frequency60,
        Frequency70,
        Frequency85,
        Frequency100,
        NoChange
    };

    public enum Frequency2
    {
        NoChange,
        Frequency60,
        Frequency70,
        Frequency85,
        Frequency100,
        Frequency120,
    };

    public enum InterfaceKind
    {
        Normal,
        None,
        Arrows,
        Code,
        Text
    };

    /// <summary>
    /// Describes the different keyboard key types.
    /// </summary>
    public enum KeyboardEventType : int
    {
        KeyBackspace = 8,
        KeyEnter = 13,
        KeyShift = 16,
        KeyControl = 17,
        KeyAlt = 18,
        KeyEscape = 27,
        KeySpace = 32,
        KeyPageUp = 33,
        KeyPageDown = 34,
        KeyEnd = 35,
        KeyHome = 36,
        KeyLeft = 37,
        KeyUp = 38,
        KeyRight = 39,
        KeyDown = 40,
        KeyInsert = 45,
        KeyDelete = 46,
        Key0 = 48,
        Key1 = 49,
        Key2 = 50,
        Key3 = 51,
        Key4 = 52,
        Key5 = 53,
        Key6 = 54,
        Key7 = 55,
        Key8 = 56,
        Key9 = 57,
        KeyA = 65,
        KeyB = 66,
        KeyC = 67,
        KeyD = 68,
        KeyE = 69,
        KeyF = 70,
        KeyG = 71,
        KeyH = 72,
        KeyI = 73,
        KeyJ = 74,
        KeyK = 75,
        KeyL = 76,
        KeyM = 77,
        KeyN = 78,
        KeyO = 79,
        KeyP = 80,
        KeyQ = 81,
        KeyR = 82,
        KeyS = 83,
        KeyT = 84,
        KeyU = 85,
        KeyV = 86,
        KeyW = 87,
        KeyX = 88,
        KeyY = 89,
        KeyZ = 90,
        KeyNumpad0 = 96,
        KeyNumpad1 = 97,
        KeyNumpad2 = 98,
        KeyNumpad3 = 99,
        KeyNumpad4 = 100,
        KeyNumpad5 = 101,
        KeyNumpad6 = 102,
        KeyNumpad7 = 103,
        KeyNumpad8 = 104,
        KeyNumpad9 = 105,
        KeyMultiply = 106,
        KeyAdd = 107,
        KeySubtract = 109,
        KeyDecimal = 110,
        KeyDivide = 111,
        KeyF1 = 112,
        KeyF2 = 113,
        KeyF3 = 114,
        KeyF4 = 115,
        KeyF5 = 116,
        KeyF6 = 117,
        KeyF7 = 118,
        KeyF8 = 119,
        KeyF9 = 120,
        KeyF10 = 121,
        KeyF11 = 122,
        KeyF12 = 123
    };

    public enum MomentType
    {
        Begin,
        Middle,
        End
    };

    public enum MouseEventType
    {
        LeftButton = 0,
        RightButton = 1,
        MiddleButton = 2,
        NoButton = 3,
        LeftPress = 4,
        RightPress = 5,
        MiddlePress = 6,
        LeftRelease = 7,
        RightRelease = 8,
        MiddleRelease = 9,
        MouseEnter = 10,
        MouseLeave = 11,
        Joystick1Left = 16,
        Joystick1Right = 17,
        Joystick1Up = 18,
        Joystick1Down = 19,
        Joystick1Button1 = 21,
        Joystick1Button2 = 22,
        Joystick1Button3 = 23,
        Joystick1Button4 = 24,
        Joystick1Button5 = 25,
        Joystick1Button6 = 26,
        Joystick1Button7 = 27,
        Joystick1Button8 = 28,
        Joystick2Left = 31,
        Joystick2Right = 32,
        Joystick2Up = 33,
        Joystick2Down = 34,
        Joystick2Button1 = 36,
        Joystick2Button2 = 37,
        Joystick2Button3 = 38,
        Joystick2Button4 = 39,
        Joystick2Button5 = 40,
        Joystick2Button6 = 41,
        Joystick2Button7 = 42,
        Joystick2Button8 = 43,
        GlobalLeftButton = 50,
        GlobalRightButton = 51,
        GlobalMiddleButton = 52,
        GlobalLeftPress = 53,
        GlobalRightPress = 54,
        GlobalMiddlePress = 55,
        GlobalLeftRelease = 56,
        GlobalRightRelease = 57,
        GlobalMiddleRelease = 58,
        MouseWheelUp = 60,
        MouseWheelDown = 61,
    };

    /// <summary>
    /// Describes the other event types.
    /// </summary>
    public enum OtherEventType
    {
        OutsideBoundary = 0,
        BoundaryCollision = 1,
        GameStart = 2,
        GameEnd = 3,
        RoomStart = 4,
        RoomEnd = 5,
        NoMoreLives = 6,
        EndOfAnimation = 7,
        EndOfPath = 8,
        NoMoreHealth = 9,
        User0 = 10,
        User1 = 11,
        User2 = 12,
        User3 = 13,
        User4 = 14,
        User5 = 15,
        User6 = 16,
        User7 = 17,
        User8 = 18,
        User9 = 19,
        User10 = 20,
        User11 = 21,
        User12 = 22,
        User13 = 23,
        User14 = 24,
        User15 = 25
    };

    public enum Priority
    {
        Normal,
        High,
        Highest
    };

    public enum ProgressBarType
    {
        None,
        Default,
        UserDefined
    };

    public enum ProjectNodeType
    {
		Root,
        Parent,
        Group,
        Child
    }

    public enum Resolution1
    {
        Resolution640x480,
        Resolution800x600,
        Resolution1024x768,
        Resolution1280x1024,
        Resolution1600x1200,
        Resolution320x240,
        NoChange
    };

    public enum Resolution2
    {
        NoChange,
        Resolution320x240,
        Resolution640x480,
        Resolution800x600,
        Resolution1024x768,
        Resolution1280x1024,
        Resolution1600x1200
    };

    public enum ResourceType
    {
        Objects = 1,
        Sprites = 2,
        Sounds = 3,
        Rooms = 4,
        Backgrounds = 6,
        Scripts = 7,
        Paths = 8,
        DataFonts = 9,
        GameInformation = 10,
        Settings = 11,
        Timelines = 12,
        Extensions = 13,
    };

    public enum ResultType
    {
        String = 1,
        Real
    };

    public enum Shape
    {
        Precise,
        Rectangle,
        Disk,
        Diamond
    };

    public enum SoundFormat
    {
        None = -1,
        Wave,
        Midi,
        Unknown = 10
    };

    public enum SoundType
    {
        Normal,
        Background,
        ThreeDimensional,
        Multimedia
    };

    /// <summary>
    /// Describes the different step event types.
    /// </summary>
    public enum StepEventType
    {
        StepNormal = 0,
        StepBegin = 1,
        StepEnd = 2
    };

    public enum SubEventType
    {
        Step,
        Mouse,
        Keyboard,
        Other
    };

    public enum TabSetting
    {
        Objects,
        Settings,
        Tiles,
        Backgrounds,
        Views
    };
}
