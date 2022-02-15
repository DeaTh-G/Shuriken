﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using XNCPLib.XNCP;
using XNCPLib.Misc;
using XNCPLib.SWIF;
using Shuriken.ViewModels;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Collections.ObjectModel;

namespace Shuriken.Models
{
    public class UICast : INotifyPropertyChanged, ICastContainer
    {
        private string name;
        public string Name
        { 
            get { return name; }
            set
            {
                if (!String.IsNullOrEmpty(value))
                    name = value;
            }
        }

        public uint Field00 { get; set; }
        public DrawType Type { get; set; }

        public bool IsEnabled { get; set; }

        public Vector2 TopLeft { get; set; }
        public Vector2 BottomLeft { get; set; }
        public Vector2 TopRight { get; set; }
        public Vector2 BottomRight { get; set; }

        public uint Field2C { get; set; }
        public uint Field34 { get; set; }
        public uint Flags { get; set; }
        public uint Field3C { get; set; }

        public UIFont Font { get; set; }
        public string FontCharacters { get; set; }

        public uint Field4C { get; set; }
        public uint Width { get; set; }
        public uint Height { get; set; }
        public uint Field58 { get; set; }
        public uint Field5C { get; set; }

        public Vector2 Offset { get; set; }
        public float Field68 { get; set; }
        public float Field6C { get; set; }
        public uint Field70 { get; set; }
        public int InfoField00 { get; set; }


        public Vector2 Translation { get; set; }
        public float Rotation { get; set; }
        public Vector2 Scale { get; set; }

        public float InfoField18 { get; set; }
        public Color Color { get ; set; }
        public Color GradientTopLeft { get; set; }
        public Color GradientBottomLeft { get; set; }
        public Color GradientTopRight { get; set; }
        public Color GradientBottomRight { get; set; }

        public uint InfoField30 { get; set; }
        public uint InfoField34 { get; set; }
        public uint InfoField38 { get; set; }

        public bool Visible { get; set; }
        public int ZIndex { get; set; }

        public ObservableCollection<int> Sprites { get; set; }
        public ObservableCollection<UICast> Children { get; set; }

        public void AddCast(UICast cast)
        {
            Children.Add(cast);
        }

        public void RemoveCast(UICast cast)
        {
            Children.Remove(cast);
        }

        public UICast(Cast cast, string name, int index)
        {
            Name = name;
            Field00 = cast.Field00;
            Type = (DrawType)cast.Field04;
            IsEnabled = cast.IsEnabled != 0;
            Visible = true;
            ZIndex = index;
            Children = new ObservableCollection<UICast>();

            TopLeft = new Vector2(cast.TopLeft);
            BottomLeft = new Vector2(cast.BottomLeft);
            TopRight = new Vector2(cast.TopRight);
            BottomRight = new Vector2(cast.BottomRight);

            Field2C = cast.Field2C;
            Field34 = cast.Field34;
            Flags = cast.Field38;
            Field3C = cast.Field3C;

            Font = null;
            FontCharacters = cast.FontCharacters;

            Field4C = cast.Field4C;
            Width = cast.Width;
            Height = cast.Height;
            Field58 = cast.Field58;
            Field5C = cast.Field5C;

            Offset = new Vector2(cast.Offset);

            Field68 = cast.Field68;
            Field6C = cast.Field6C;
            Field70 = cast.Field70;

            InfoField00 = cast.CastInfoData.Field00;
            Translation = new Vector2(cast.CastInfoData.Translation);
            Rotation = cast.CastInfoData.Rotation;
            Scale = new Vector2(cast.CastInfoData.Scale);
            InfoField18 = cast.CastInfoData.Field18;
            Color = new Color(cast.CastInfoData.Color);
            GradientTopLeft = new Color(cast.CastInfoData.GradientTopLeft);
            GradientBottomLeft = new Color(cast.CastInfoData.GradientBottomLeft);
            GradientTopRight = new Color(cast.CastInfoData.GradientTopRight);
            GradientBottomRight = new Color(cast.CastInfoData.GradientBottomRight);
            InfoField30 = cast.CastInfoData.Field30;
            InfoField34 = cast.CastInfoData.Field34;
            InfoField38 = cast.CastInfoData.Field38;

            Sprites = new ObservableCollection<int>();
            for (int i = 0; i < 32; ++i)
                Sprites.Add(-1);
        }

        public UICast(SWCastNode castnode, SWCell cell, System.Numerics.Vector2 framesize, string name, int index)
        {
            Name = name;
            IsEnabled = true;
            Visible = true;
            ZIndex = index;
            Children = new ObservableCollection<UICast>();

            TopLeft = new Vector2();
            BottomLeft = new Vector2();
            TopRight = new Vector2();
            BottomRight = new Vector2();

            Offset = new Vector2(cell.CellInfo.Position.X / framesize.X, -(cell.CellInfo.Position.Y / framesize.Y));

            Translation = new Vector2();
            if (index == 0)
                Translation = new Vector2(0.5f, 0.5f);

            Rotation = cell.CellInfo.Rotation * 360 / ushort.MaxValue;
            Scale = new Vector2(cell.CellInfo.Scale.X, cell.CellInfo.Scale.Y);
            Color = new Color(Utilities.ReverseColor(cell.Color));
            GradientTopLeft = new Color(255, 255, 255, 255);
            GradientBottomLeft = new Color(255, 255, 255, 255);
            GradientTopRight = new Color(255, 255, 255, 255);
            GradientBottomRight = new Color(255, 255, 255, 255);

            if ((castnode.Flags & 0xF) == (int)SWCastNode.EFlags.eFlags_ImageCast)
            {
                Type = (castnode.Cast.ImageCast.Flags & SWImageCast.EFlags.eFlags_UseFont) != 0 ? DrawType.Font : DrawType.Sprite;
                
                Vector2 anchorPoint = new Vector2();
                anchorPoint.X = (castnode.Cast.ImageCast.AnchorPoint.X != 0 ? castnode.Cast.ImageCast.AnchorPoint.X : castnode.Cast.ImageCast.Width) / framesize.X;
                anchorPoint.Y = (castnode.Cast.ImageCast.AnchorPoint.Y != 0 ? castnode.Cast.ImageCast.AnchorPoint.Y : castnode.Cast.ImageCast.Height) / framesize.Y;
                if ((castnode.Cast.ImageCast.Flags & SWImageCast.EFlags.eFlags_AnchorRight) == SWImageCast.EFlags.eFlags_AnchorRight)
                {
                    TopRight = new Vector2(anchorPoint.X, 0);
                    BottomRight = new Vector2(anchorPoint.X, 0);
                }
                else if ((castnode.Cast.ImageCast.Flags & SWImageCast.EFlags.eFlags_AnchorLeft) == SWImageCast.EFlags.eFlags_AnchorLeft)
                {
                    TopLeft = new Vector2(anchorPoint.X, 0);
                    BottomLeft = new Vector2(anchorPoint.X, 0);
                }
                else if ((castnode.Cast.ImageCast.Flags & SWImageCast.EFlags.eFlags_AnchorTopRight) == SWImageCast.EFlags.eFlags_AnchorTopRight)
                {
                    TopRight = new Vector2(anchorPoint.X, anchorPoint.Y);
                }
                else if ((castnode.Cast.ImageCast.Flags & SWImageCast.EFlags.eFlags_AnchorTopLeft) == SWImageCast.EFlags.eFlags_AnchorTopLeft)
                {
                    TopLeft = new Vector2(anchorPoint.X, 0);
                    TopRight = new Vector2(0, anchorPoint.Y);
                }

                //Flags = (uint)cast.Flags;

                Font = null;
                FontCharacters = castnode.Cast.ImageCast.FontInfo.Characters;

                Width = (uint)castnode.Cast.ImageCast.Width;
                Height = (uint)castnode.Cast.ImageCast.Height;

                if ((castnode.Cast.ImageCast.Flags & SWImageCast.EFlags.eFlags_RotateLeft) == SWImageCast.EFlags.eFlags_RotateLeft)
                {
                    Width = (uint)castnode.Cast.ImageCast.Height;
                    Height = (uint)castnode.Cast.ImageCast.Width;
                    Rotation += 90;
                }

                if ((castnode.Cast.ImageCast.Flags & SWImageCast.EFlags.eFlags_FlipHorizontally) == SWImageCast.EFlags.eFlags_FlipHorizontally)
                {
                    Scale.X = -Scale.X;
                }
                else if ((castnode.Cast.ImageCast.Flags & SWImageCast.EFlags.eFlags_FlipVertically) == SWImageCast.EFlags.eFlags_FlipVertically)
                {
                    Scale.Y = -Scale.Y;
                }
                else if ((castnode.Cast.ImageCast.Flags & SWImageCast.EFlags.eFlags_FlipHorizontallyAndVertically) == SWImageCast.EFlags.eFlags_FlipHorizontallyAndVertically)
                {
                    Scale.X = -Scale.X;
                    Scale.Y = -Scale.Y;
                }

                GradientTopLeft = new Color(Utilities.ReverseColor(castnode.Cast.ImageCast.GradientTopLeft));
                GradientBottomLeft = new Color(Utilities.ReverseColor(castnode.Cast.ImageCast.GradientBottomLeft));
                GradientTopRight = new Color(Utilities.ReverseColor(castnode.Cast.ImageCast.GradientTopRight));
                GradientBottomRight = new Color(Utilities.ReverseColor(castnode.Cast.ImageCast.GradientBottomRight));
            }

            Sprites = new ObservableCollection<int>();
            for (int i = 0; i < 64; ++i)
                Sprites.Add(-1);
        }

        public UICast()
        {
            Name = "Cast";
            Field00 = 0;
            Type = DrawType.Sprite;
            IsEnabled = true;
            Visible = true;
            ZIndex = 0;
            Children = new ObservableCollection<UICast>();

            TopLeft = new Vector2();
            BottomLeft = new Vector2();
            TopRight = new Vector2();
            BottomRight = new Vector2();

            Field2C = 0;
            Field34 = 0;
            Flags = 0;
            Field3C = 0;

            Font = null;
            FontCharacters = "";

            Field4C = 0;
            Width = 0;
            Height = 0;
            Field58 = 0;
            Field5C = 0;

            Offset = new Vector2();

            Field68 = 0;
            Field6C = 0;
            Field70 = 0;

            InfoField00 = 0;
            Translation = new Vector2();
            Rotation = 0;
            Scale = new Vector2(1.0f, 1.0f);
            InfoField18 = 0;
            Color = new Color(255, 255, 255, 255);
            GradientTopLeft = new Color(255, 255, 255, 255);
            GradientBottomLeft = new Color(255, 255, 255, 255);
            GradientTopRight = new Color(255, 255, 255, 255);
            GradientBottomRight = new Color(255, 255, 255, 255);
            InfoField30 = 0;
            InfoField34 = 0;
            InfoField38 = 0;

            Sprites = new ObservableCollection<int>();
            for (int i = 0; i < 32; ++i)
                Sprites.Add(-1);
        }

        public UICast(UICast c)
        {
            Name = name;
            Field00 = c.Field00;
            Type = c.Type;
            IsEnabled = c.IsEnabled;
            Visible = true;
            ZIndex = ZIndex;
            Children = new ObservableCollection<UICast>(c.Children);

            TopLeft = new Vector2(c.TopLeft);
            BottomLeft = new Vector2(c.BottomLeft);
            TopRight = new Vector2(c.TopRight);
            BottomRight = new Vector2(c.BottomRight);

            Field2C = c.Field2C;
            Field34 = c.Field34;
            Flags = c.Flags;
            Field3C = c.Field3C;

            Font = null;
            FontCharacters = c.FontCharacters;

            Field4C = c.Field4C;
            Width = c.Width;
            Height = c.Height;
            Field58 = c.Field58;
            Field5C = c.Field5C;

            Offset = new Vector2(c.Offset);

            Field68 = c.Field68;
            Field6C = c.Field6C;
            Field70 = c.Field70;

            InfoField00 = c.InfoField00;
            Translation = new Vector2(c.Translation);
            Rotation = c.Rotation;
            Scale = new Vector2(c.Scale);
            InfoField18 = c.InfoField18;
            Color = new Color(c.Color);
            GradientTopLeft = new Color(c.GradientTopLeft);
            GradientBottomLeft = new Color(c.GradientBottomLeft);
            GradientTopRight = new Color(c.GradientTopRight);
            GradientBottomRight = new Color(c.GradientBottomRight);
            InfoField30 = c.InfoField30;
            InfoField34 = c.InfoField34;
            InfoField38 = c.InfoField38;

            Sprites = new ObservableCollection<int>(c.Sprites);
        }


        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
