// Copyright (c) Hugues Valois. All rights reserved.
// Licensed under the X11 license. See LICENSE in the project root for license information.

namespace Woohoo.Agi.Player.Backend.Sdl;

using System.Runtime.CompilerServices;

#if USE_SDL
internal static partial class NativeMethods
{
    public const int SDL_INIT_TIMER = 1;

    public const int SDL_INIT_AUDIO = 16;

    public const int SDL_INIT_VIDEO = 32;

    public const int SDL_INIT_CDROM = 256;

    public const int SDL_INIT_JOYSTICK = 512;

    public const int SDL_INIT_NOPARACHUTE = 1048576;

    public const int SDL_INIT_EVENTTHREAD = 16777216;

    public const int SDL_INIT_EVERYTHING = 65535;

    public const byte SDL_PRESSED = 1;

    public const byte SDL_RELEASED = 0;

    public const int SDL_ALLEVENTS = -1;

    public const int SDL_QUERY = -1;

    public const int SDL_IGNORE = 0;

    public const int SDL_DISABLE = 0;

    public const int SDL_ENABLE = 1;

    public const int SDL_ALL_HOTKEYS = -1;

    public const int SDL_DEFAULT_REPEAT_DELAY = 500;

    public const int SDL_DEFAULT_REPEAT_INTERVAL = 30;

    public const byte SDL_BUTTON_LEFT = 1;

    public const byte SDL_BUTTON_MIDDLE = 2;

    public const byte SDL_BUTTON_RIGHT = 3;

    public const byte SDL_BUTTON_WHEELUP = 4;

    public const byte SDL_BUTTON_WHEELDOWN = 5;

    public const byte SDL_BUTTON_X1 = 6;

    public const byte SDL_BUTTON_X2 = 7;

    public const byte SDL_BUTTON_LMASK = 1;

    public const byte SDL_BUTTON_MMASK = 2;

    public const byte SDL_BUTTON_RMASK = 4;

    public const byte SDL_BUTTON_X1MASK = 32;

    public const byte SDL_BUTTON_X2MASK = 64;

    public const int SDL_ANYFORMAT = 268435456;

    public const int SDL_HWPALETTE = 536870912;

    public const int SDL_DOUBLEBUF = 1073741824;

    public const int SDL_FULLSCREEN = -2147483648;

    public const int SDL_RESIZABLE = 16;

    public const byte SDL_LOGPAL = 1;

    public const byte SDL_PHYSPAL = 2;

    private const string nativeLibName = "SDL";

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void SDL_AudioCallback(IntPtr userdata, IntPtr stream, int len);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate int SDL_EventFilter([Out] SDL_Event evt);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate int SDL_ThreadCallback();

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate int SDL_TimerCallback(int interval);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate int SDL_NewTimerCallback(int interval);

    [LibraryImport(nativeLibName)]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial int SDL_Init(int flags);

    [LibraryImport(nativeLibName)]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial void SDL_QuitSubSystem(int flags);

    [LibraryImport(nativeLibName)]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial void SDL_Quit();

    [LibraryImport(nativeLibName)]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial int SDL_OpenAudio(IntPtr desired, IntPtr obtained);

    [LibraryImport(nativeLibName)]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial void SDL_PauseAudio(int pause_on);

    [LibraryImport(nativeLibName)]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial void SDL_LockAudio();

    [LibraryImport(nativeLibName)]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial void SDL_UnlockAudio();

    [LibraryImport(nativeLibName)]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial void SDL_CloseAudio();

    [LibraryImport(nativeLibName)]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial void SDL_PumpEvents();

    [LibraryImport(nativeLibName)]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial int SDL_PeepEvents([In][Out] SDL_Event[] events, int numevents, int action, int mask);

    [LibraryImport(nativeLibName)]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial int SDL_PollEvent(out SDL_Event sdlEvent);

    [LibraryImport(nativeLibName)]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial int SDL_PushEvent(out SDL_Event evt);

    [LibraryImport(nativeLibName)]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial int SDL_EventState(byte type, int state);

    [LibraryImport(nativeLibName)]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial int SDL_NumJoysticks();

    [LibraryImport(nativeLibName)]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial IntPtr SDL_JoystickOpen(int device_index);

    [LibraryImport(nativeLibName)]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial int SDL_JoystickEventState(int state);

    [LibraryImport(nativeLibName)]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial int SDL_EnableUNICODE(int enable);

    [LibraryImport(nativeLibName)]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial int SDL_EnableKeyRepeat(int rate, int delay);

    [LibraryImport(nativeLibName)]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial byte SDL_GetMouseState(out int x, out int y);

    [LibraryImport(nativeLibName)]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial IntPtr SDL_CreateThread(SDL_ThreadCallback fn, IntPtr data);

    [LibraryImport(nativeLibName)]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial void SDL_WaitThread(IntPtr thread, out int status);

    [LibraryImport(nativeLibName)]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial int SDL_GetTicks();

    [LibraryImport(nativeLibName)]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial void SDL_Delay(int ms);

    [LibraryImport(nativeLibName)]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial IntPtr SDL_SetVideoMode(int width, int height, int bpp, int flags);

    [LibraryImport(nativeLibName)]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial void SDL_UpdateRect(IntPtr screen, int x, int y, int w, int h);

    [LibraryImport(nativeLibName)]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial int SDL_SetPalette(IntPtr surface, int flags, [In][Out] SDL_Color[] colors, int firstcolor, int ncolors);

    [LibraryImport(nativeLibName)]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial void SDL_SetClipRect(IntPtr surface, ref SDL_Rect rect);

    [LibraryImport(nativeLibName)]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial int SDL_FillRect(IntPtr surface, ref SDL_Rect rect, int color);

    [LibraryImport(nativeLibName, StringMarshalling = StringMarshalling.Utf8)]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial void SDL_WM_SetCaption(string title, string icon);

    [LibraryImport(nativeLibName)]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial int SDL_WM_IconifyWindow();

    [LibraryImport(nativeLibName)]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial int SDL_WM_ToggleFullScreen(IntPtr surface);

    public static string SDL_GetError()
    {
        return Marshal.PtrToStringAnsi(__SDL_GetError());
    }

    [LibraryImport(nativeLibName, EntryPoint = "SDL_GetError")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    private static partial IntPtr __SDL_GetError();

    [StructLayout(LayoutKind.Sequential, Pack = 4)]
    public struct SDL_AudioSpec
    {
        public int freq;

        public short format;

        public byte channels;

        public byte silence;

        public short samples;

        public short padding;

        public int size;

        public IntPtr callback;

        public object userdata;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 4)]
    public struct SDL_ActiveEvent
    {
        public byte type;

        public byte gain;

        public byte state;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 4)]
    public struct SDL_KeyboardEvent
    {
        public byte type;

        public byte which;

        public byte state;

        public SDL_keysym keysym;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 4)]
    public struct SDL_MouseMotionEvent
    {
        public byte type;

        public byte which;

        public byte state;

        public short x;

        public short y;

        public short xrel;

        public short yrel;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 4)]
    public struct SDL_MouseButtonEvent
    {
        public byte type;

        public byte which;

        public byte button;

        public byte state;

        public short x;

        public short y;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 4)]
    public struct SDL_JoyAxisEvent
    {
        public byte type;

        public byte which;

        public byte axis;

        public short val;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 4)]
    public struct SDL_JoyBallEvent
    {
        public byte type;

        public byte which;

        public byte ball;

        public short xrel;

        public short yrel;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 4)]
    public struct SDL_JoyHatEvent
    {
        public byte type;

        public byte which;

        public byte hat;

        public byte val;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 4)]
    public struct SDL_JoyButtonEvent
    {
        public byte type;

        public byte which;

        public byte button;

        public byte state;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 4)]
    public struct SDL_ResizeEvent
    {
        public byte type;

        public int w;

        public int h;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 4)]
    public struct SDL_ExposeEvent
    {
        public byte type;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 4)]
    public struct SDL_QuitEvent
    {
        public byte type;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 4)]
    public struct SDL_UserEvent
    {
        public byte type;

        public int code;

        public IntPtr data1;

        public IntPtr data2;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 4)]
    public struct SDL_SysWMEvent
    {
        public byte type;

        public IntPtr msg;
    }

    [StructLayout(LayoutKind.Explicit)]
    public struct SDL_Event
    {
        [FieldOffset(0)]
        public byte type;

        [FieldOffset(0)]
        public SDL_ActiveEvent active;

        [FieldOffset(0)]
        public SDL_KeyboardEvent key;

        [FieldOffset(0)]
        public SDL_MouseMotionEvent motion;

        [FieldOffset(0)]
        public SDL_MouseButtonEvent button;

        [FieldOffset(0)]
        public SDL_JoyAxisEvent jaxis;

        [FieldOffset(0)]
        public SDL_JoyBallEvent jball;

        [FieldOffset(0)]
        public SDL_JoyHatEvent jhat;

        [FieldOffset(0)]
        public SDL_JoyButtonEvent jbutton;

        [FieldOffset(0)]
        public SDL_ResizeEvent resize;

        [FieldOffset(0)]
        public SDL_ExposeEvent expose;

        [FieldOffset(0)]
        public SDL_QuitEvent quit;

        [FieldOffset(0)]
        public SDL_UserEvent user;

        [FieldOffset(0)]
        public SDL_SysWMEvent syswm;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 4)]
    public struct SDL_keysym
    {
        public byte scancode;

        public int sym;

        public int mod;

        public short unicode;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 4)]
    public struct SDL_Rect
    {
        public short x;

        public short y;

        public short w;

        public short h;

        public SDL_Rect(short x, short y, short w, short h)
        {
            this.x = x;
            this.y = y;
            this.w = w;
            this.h = h;
        }

        public override readonly string ToString()
        {
            return string.Concat(new object[]
            {
                "x: ",
                this.x,
                ", y: ",
                this.y,
                ", w: ",
                this.w,
                ", h: ",
                this.h,
            });
        }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 4)]
    public struct SDL_Color
    {
        public byte r;

        public byte g;

        public byte b;

        public byte unused;

        public SDL_Color(byte r, byte g, byte b)
        {
            this.r = r;
            this.g = g;
            this.b = b;
            this.unused = 0;
        }

        public SDL_Color(byte r, byte g, byte b, byte a)
        {
            this.r = r;
            this.g = g;
            this.b = b;
            this.unused = a;
        }
    }

#if X86
    [StructLayout(LayoutKind.Sequential, Pack = 4)]
#elif X64
    [StructLayout(LayoutKind.Sequential, Pack = 8)]
#endif
    public struct SDL_Surface
    {
        public int flags;

        public IntPtr format;

        public int w;

        public int h;

        public short pitch;

        public IntPtr pixels;

        public int offset;

        public IntPtr hwdata;

        public SDL_Rect clip_rect;

        public int unused1;

        public int locked;

        public IntPtr map;

        public int format_version;

        public int refcount;
    }

    public static class SDL_EventType
    {
        public const int SDL_NOEVENT = 0;

        public const int SDL_ACTIVEEVENT = 1;

        public const int SDL_KEYDOWN = 2;

        public const int SDL_KEYUP = 3;

        public const int SDL_MOUSEMOTION = 4;

        public const int SDL_MOUSEBUTTONDOWN = 5;

        public const int SDL_MOUSEBUTTONUP = 6;

        public const int SDL_JOYAXISMOTION = 7;

        public const int SDL_JOYBALLMOTION = 8;

        public const int SDL_JOYHATMOTION = 9;

        public const int SDL_JOYBUTTONDOWN = 10;

        public const int SDL_JOYBUTTONUP = 11;

        public const int SDL_QUIT = 12;

        public const int SDL_SYSWMEVENT = 13;

        public const int SDL_EVENT_RESERVEDA = 14;

        public const int SDL_EVENT_RESERVEDB = 15;

        public const int SDL_VIDEORESIZE = 16;

        public const int SDL_VIDEOEXPOSE = 17;

        public const int SDL_EVENT_RESERVED2 = 18;

        public const int SDL_EVENT_RESERVED3 = 19;

        public const int SDL_EVENT_RESERVED4 = 20;

        public const int SDL_EVENT_RESERVED5 = 21;

        public const int SDL_EVENT_RESERVED6 = 22;

        public const int SDL_EVENT_RESERVED7 = 23;

        public const int SDL_USEREVENT = 24;

        public const int SDL_NUMEVENTS = 32;
    }

    public static class SDL_eventaction
    {
        public const int SDL_ADDEVENT = 0;

        public const int SDL_PEEKEVENT = 1;

        public const int SDL_GETEVENT = 2;
    }

    public static class SDL_Keycode
    {
        public const int SDLK_UNKNOWN = 0;

        public const int SDLK_FIRST = 0;

        public const int SDLK_BACKSPACE = 8;

        public const int SDLK_TAB = 9;

        public const int SDLK_CLEAR = 12;

        public const int SDLK_RETURN = 13;

        public const int SDLK_PAUSE = 19;

        public const int SDLK_ESCAPE = 27;

        public const int SDLK_SPACE = 32;

        public const int SDLK_EXCLAIM = 33;

        public const int SDLK_QUOTEDBL = 34;

        public const int SDLK_HASH = 35;

        public const int SDLK_DOLLAR = 36;

        public const int SDLK_AMPERSAND = 38;

        public const int SDLK_QUOTE = 39;

        public const int SDLK_LEFTPAREN = 40;

        public const int SDLK_RIGHTPAREN = 41;

        public const int SDLK_ASTERISK = 42;

        public const int SDLK_PLUS = 43;

        public const int SDLK_COMMA = 44;

        public const int SDLK_MINUS = 45;

        public const int SDLK_PERIOD = 46;

        public const int SDLK_SLASH = 47;

        public const int SDLK_0 = 48;

        public const int SDLK_1 = 49;

        public const int SDLK_2 = 50;

        public const int SDLK_3 = 51;

        public const int SDLK_4 = 52;

        public const int SDLK_5 = 53;

        public const int SDLK_6 = 54;

        public const int SDLK_7 = 55;

        public const int SDLK_8 = 56;

        public const int SDLK_9 = 57;

        public const int SDLK_COLON = 58;

        public const int SDLK_SEMICOLON = 59;

        public const int SDLK_LESS = 60;

        public const int SDLK_EQUALS = 61;

        public const int SDLK_GREATER = 62;

        public const int SDLK_QUESTION = 63;

        public const int SDLK_AT = 64;

        public const int SDLK_LEFTBRACKET = 91;

        public const int SDLK_BACKSLASH = 92;

        public const int SDLK_RIGHTBRACKET = 93;

        public const int SDLK_CARET = 94;

        public const int SDLK_UNDERSCORE = 95;

        public const int SDLK_BACKQUOTE = 96;

        public const int SDLK_a = 97;

        public const int SDLK_b = 98;

        public const int SDLK_c = 99;

        public const int SDLK_d = 100;

        public const int SDLK_e = 101;

        public const int SDLK_f = 102;

        public const int SDLK_g = 103;

        public const int SDLK_h = 104;

        public const int SDLK_i = 105;

        public const int SDLK_j = 106;

        public const int SDLK_k = 107;

        public const int SDLK_l = 108;

        public const int SDLK_m = 109;

        public const int SDLK_n = 110;

        public const int SDLK_o = 111;

        public const int SDLK_p = 112;

        public const int SDLK_q = 113;

        public const int SDLK_r = 114;

        public const int SDLK_s = 115;

        public const int SDLK_t = 116;

        public const int SDLK_u = 117;

        public const int SDLK_v = 118;

        public const int SDLK_w = 119;

        public const int SDLK_x = 120;

        public const int SDLK_y = 121;

        public const int SDLK_z = 122;

        public const int SDLK_DELETE = 127;

        public const int SDLK_WORLD_0 = 160;

        public const int SDLK_WORLD_1 = 161;

        public const int SDLK_WORLD_2 = 162;

        public const int SDLK_WORLD_3 = 163;

        public const int SDLK_WORLD_4 = 164;

        public const int SDLK_WORLD_5 = 165;

        public const int SDLK_WORLD_6 = 166;

        public const int SDLK_WORLD_7 = 167;

        public const int SDLK_WORLD_8 = 168;

        public const int SDLK_WORLD_9 = 169;

        public const int SDLK_WORLD_10 = 170;

        public const int SDLK_WORLD_11 = 171;

        public const int SDLK_WORLD_12 = 172;

        public const int SDLK_WORLD_13 = 173;

        public const int SDLK_WORLD_14 = 174;

        public const int SDLK_WORLD_15 = 175;

        public const int SDLK_WORLD_16 = 176;

        public const int SDLK_WORLD_17 = 177;

        public const int SDLK_WORLD_18 = 178;

        public const int SDLK_WORLD_19 = 179;

        public const int SDLK_WORLD_20 = 180;

        public const int SDLK_WORLD_21 = 181;

        public const int SDLK_WORLD_22 = 182;

        public const int SDLK_WORLD_23 = 183;

        public const int SDLK_WORLD_24 = 184;

        public const int SDLK_WORLD_25 = 185;

        public const int SDLK_WORLD_26 = 186;

        public const int SDLK_WORLD_27 = 187;

        public const int SDLK_WORLD_28 = 188;

        public const int SDLK_WORLD_29 = 189;

        public const int SDLK_WORLD_30 = 190;

        public const int SDLK_WORLD_31 = 191;

        public const int SDLK_WORLD_32 = 192;

        public const int SDLK_WORLD_33 = 193;

        public const int SDLK_WORLD_34 = 194;

        public const int SDLK_WORLD_35 = 195;

        public const int SDLK_WORLD_36 = 196;

        public const int SDLK_WORLD_37 = 197;

        public const int SDLK_WORLD_38 = 198;

        public const int SDLK_WORLD_39 = 199;

        public const int SDLK_WORLD_40 = 200;

        public const int SDLK_WORLD_41 = 201;

        public const int SDLK_WORLD_42 = 202;

        public const int SDLK_WORLD_43 = 203;

        public const int SDLK_WORLD_44 = 204;

        public const int SDLK_WORLD_45 = 205;

        public const int SDLK_WORLD_46 = 206;

        public const int SDLK_WORLD_47 = 207;

        public const int SDLK_WORLD_48 = 208;

        public const int SDLK_WORLD_49 = 209;

        public const int SDLK_WORLD_50 = 210;

        public const int SDLK_WORLD_51 = 211;

        public const int SDLK_WORLD_52 = 212;

        public const int SDLK_WORLD_53 = 213;

        public const int SDLK_WORLD_54 = 214;

        public const int SDLK_WORLD_55 = 215;

        public const int SDLK_WORLD_56 = 216;

        public const int SDLK_WORLD_57 = 217;

        public const int SDLK_WORLD_58 = 218;

        public const int SDLK_WORLD_59 = 219;

        public const int SDLK_WORLD_60 = 220;

        public const int SDLK_WORLD_61 = 221;

        public const int SDLK_WORLD_62 = 222;

        public const int SDLK_WORLD_63 = 223;

        public const int SDLK_WORLD_64 = 224;

        public const int SDLK_WORLD_65 = 225;

        public const int SDLK_WORLD_66 = 226;

        public const int SDLK_WORLD_67 = 227;

        public const int SDLK_WORLD_68 = 228;

        public const int SDLK_WORLD_69 = 229;

        public const int SDLK_WORLD_70 = 230;

        public const int SDLK_WORLD_71 = 231;

        public const int SDLK_WORLD_72 = 232;

        public const int SDLK_WORLD_73 = 233;

        public const int SDLK_WORLD_74 = 234;

        public const int SDLK_WORLD_75 = 235;

        public const int SDLK_WORLD_76 = 236;

        public const int SDLK_WORLD_77 = 237;

        public const int SDLK_WORLD_78 = 238;

        public const int SDLK_WORLD_79 = 239;

        public const int SDLK_WORLD_80 = 240;

        public const int SDLK_WORLD_81 = 241;

        public const int SDLK_WORLD_82 = 242;

        public const int SDLK_WORLD_83 = 243;

        public const int SDLK_WORLD_84 = 244;

        public const int SDLK_WORLD_85 = 245;

        public const int SDLK_WORLD_86 = 246;

        public const int SDLK_WORLD_87 = 247;

        public const int SDLK_WORLD_88 = 248;

        public const int SDLK_WORLD_89 = 249;

        public const int SDLK_WORLD_90 = 250;

        public const int SDLK_WORLD_91 = 251;

        public const int SDLK_WORLD_92 = 252;

        public const int SDLK_WORLD_93 = 253;

        public const int SDLK_WORLD_94 = 254;

        public const int SDLK_WORLD_95 = 255;

        public const int SDLK_KP0 = 256;

        public const int SDLK_KP1 = 257;

        public const int SDLK_KP2 = 258;

        public const int SDLK_KP3 = 259;

        public const int SDLK_KP4 = 260;

        public const int SDLK_KP5 = 261;

        public const int SDLK_KP6 = 262;

        public const int SDLK_KP7 = 263;

        public const int SDLK_KP8 = 264;

        public const int SDLK_KP9 = 265;

        public const int SDLK_KP_PERIOD = 266;

        public const int SDLK_KP_DIVIDE = 267;

        public const int SDLK_KP_MULTIPLY = 268;

        public const int SDLK_KP_MINUS = 269;

        public const int SDLK_KP_PLUS = 270;

        public const int SDLK_KP_ENTER = 271;

        public const int SDLK_KP_EQUALS = 272;

        public const int SDLK_UP = 273;

        public const int SDLK_DOWN = 274;

        public const int SDLK_RIGHT = 275;

        public const int SDLK_LEFT = 276;

        public const int SDLK_INSERT = 277;

        public const int SDLK_HOME = 278;

        public const int SDLK_END = 279;

        public const int SDLK_PAGEUP = 280;

        public const int SDLK_PAGEDOWN = 281;

        public const int SDLK_F1 = 282;

        public const int SDLK_F2 = 283;

        public const int SDLK_F3 = 284;

        public const int SDLK_F4 = 285;

        public const int SDLK_F5 = 286;

        public const int SDLK_F6 = 287;

        public const int SDLK_F7 = 288;

        public const int SDLK_F8 = 289;

        public const int SDLK_F9 = 290;

        public const int SDLK_F10 = 291;

        public const int SDLK_F11 = 292;

        public const int SDLK_F12 = 293;

        public const int SDLK_F13 = 294;

        public const int SDLK_F14 = 295;

        public const int SDLK_F15 = 296;

        public const int SDLK_NUMLOCK = 300;

        public const int SDLK_CAPSLOCK = 301;

        public const int SDLK_SCROLLOCK = 302;

        public const int SDLK_RSHIFT = 303;

        public const int SDLK_LSHIFT = 304;

        public const int SDLK_RCTRL = 305;

        public const int SDLK_LCTRL = 306;

        public const int SDLK_RALT = 307;

        public const int SDLK_LALT = 308;

        public const int SDLK_RMETA = 309;

        public const int SDLK_LMETA = 310;

        public const int SDLK_LSUPER = 311;

        public const int SDLK_RSUPER = 312;

        public const int SDLK_MODE = 313;

        public const int SDLK_COMPOSE = 314;

        public const int SDLK_HELP = 315;

        public const int SDLK_PRINT = 316;

        public const int SDLK_SYSREQ = 317;

        public const int SDLK_BREAK = 318;

        public const int SDLK_MENU = 319;

        public const int SDLK_POWER = 320;

        public const int SDLK_EURO = 321;

        public const int SDLK_UNDO = 322;

        public const int SDLK_LAST = 323;
    }

    public static class SDL_Keymod
    {
        public const int KMOD_NONE = 0;

        public const int KMOD_LSHIFT = 1;

        public const int KMOD_RSHIFT = 2;

        public const int KMOD_SHIFT = 3;

        public const int KMOD_LCTRL = 64;

        public const int KMOD_RCTRL = 128;

        public const int KMOD_CTRL = 192;

        public const int KMOD_LALT = 256;

        public const int KMOD_RALT = 512;

        public const int KMOD_ALT = 768;

        public const int KMOD_LMETA = 1024;

        public const int KMOD_RMETA = 2048;

        public const int KMOD_META = 3072;

        public const int KMOD_NUM = 4096;

        public const int KMOD_CAPS = 8192;

        public const int KMOD_MODE = 16384;

        public const int KMOD_RESERVED = 32768;
    }

    public static class SDL_bool
    {
        public const int SDL_FALSE = 0;

        public const int SDL_TRUE = 1;
    }
}
#endif
