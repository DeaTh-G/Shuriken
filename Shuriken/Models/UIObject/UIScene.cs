using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XNCPLib.XNCP;
using XNCPLib.SWIF;
using XNCPLib.SWIF.Cast;
using System.ComponentModel;
using Shuriken.Models.Animation;
using Shuriken.Misc;
using Shuriken.ViewModels;
using System.Runtime.CompilerServices;
using System.Windows;

namespace Shuriken.Models
{
    public class UIScene : INotifyPropertyChanged, IComparable<UIScene>
    {
        private string name;
        public string Name
        {
            get => name;
            set
            {
                if (!string.IsNullOrEmpty(value))
                    name = value;
            }
        }

        public uint Field00 { get; set; }
        public float ZIndex { get; set; }
        public uint Field0C { get; set; }
        public float Field10 { get; set; }
        public float AspectRatio { get; set; }
        public float AnimationFramerate { get; set; }
        public bool Visible { get; set; }

        public ObservableCollection<Vector2> TextureSizes { get; set; }
        public ObservableCollection<UICastGroup> Groups { get; set; }
        public UIScene(Scene scene, string sceneName, TextureList texList, IEnumerable<UIFont> fonts)
        {
            Name = sceneName;
            Field00 = scene.Field00;
            ZIndex = scene.ZIndex;
            Field0C = scene.Field0C;
            Field10 = scene.Field10;
            AspectRatio = scene.AspectRatio;
            AnimationFramerate = scene.AnimationFramerate;
            TextureSizes = new ObservableCollection<Vector2>();
            Groups = new ObservableCollection<UICastGroup>();

            foreach (var texSize in scene.Data1)
            {
                TextureSizes.Add(new Vector2(texSize.X, texSize.Y));
            }

            ProcessCasts(scene, texList, fonts);
            Visible = false;
        }

        public UIScene(SWProjectNode project, SWScene scene, string sceneName, IEnumerable<TextureList> texLists, IEnumerable<UIFont> fonts)
        {
            Name = sceneName;
            AspectRatio = scene.FrameSize.X / scene.FrameSize.Y;
            AnimationFramerate = project.FrameRate;
            TextureSizes = new ObservableCollection<Vector2>();
            Groups = new ObservableCollection<UICastGroup>();

            ProcessSWCasts(scene, texLists, fonts);
            Visible = false;
        }

        public UIScene(string sceneName)
        {
            Name = sceneName;
            ZIndex = 0;
            AspectRatio = 16.0f / 9.0f;
            AnimationFramerate = 60.0f;
            Groups = new ObservableCollection<UICastGroup>();
            TextureSizes = new ObservableCollection<Vector2>();

            Visible = false;
        }

        public UIScene(UIScene s)
        {
            Name = s.Name;
            ZIndex = s.ZIndex;
            AspectRatio = s.AspectRatio;
            AnimationFramerate = s.AnimationFramerate;

            Groups = new ObservableCollection<UICastGroup>(s.Groups);
            TextureSizes = new ObservableCollection<Vector2>(s.TextureSizes);

            Visible = false;
        }

        private void ProcessCasts(Scene scene, TextureList texList, IEnumerable<UIFont> fonts)
        {
            // Create groups
            for (int g = 0; g < scene.GroupCount; ++g)
            {
                Groups.Add(new UICastGroup
                {
                    Name = "Group_" + g,
                    Field08 = scene.UICastGroups[g].Field08
                });

                // Pre-process animations
                Dictionary<int, int> entryIndexMap = new Dictionary<int, int>();
                int animIndex = 0;
                foreach (var entry in scene.AnimationDictionaries)
                {
                    Groups[g].Animations.Add(new AnimationGroup(entry.Name)
                    {
                        Field00 = scene.AnimationFrameDataList[(int)entry.Index].Field00,
                        Duration = scene.AnimationFrameDataList[(int)entry.Index].FrameCount
                    });

                    entryIndexMap.Add(animIndex++, (int)entry.Index);
                }

                // process group layers
                List<UICast> tempCasts = new List<UICast>();
                for (int c = 0; c < scene.UICastGroups[g].CastCount; ++c)
                {
                    UICast cast = new UICast(scene.UICastGroups[g].Casts[c], GetCastName(g, c, scene.CastDictionaries), c);

                    // sprite
                    if (cast.Type == DrawType.Sprite)
                    {
                        int[] castSprites = scene.UICastGroups[g].Casts[c].CastMaterialData.SubImageIndices;
                        for (int index = 0; index < cast.Sprites.Count; ++index)
                        {
                            cast.Sprites[index] = Utilities.FindSpriteIDFromNCPScene(castSprites[index], scene.SubImages, texList.Textures);
                        }
                    }
                    else if (cast.Type == DrawType.Font)
                    {
                        foreach (var font in fonts)
                        {
                            if (font.Name == scene.UICastGroups[g].Casts[c].FontName)
                                cast.Font = font;
                        }
                    }

                    tempCasts.Add(cast);
                }

                foreach (var entry in entryIndexMap)
                {
                    XNCPLib.XNCP.Animation.AnimationKeyframeData keyframeData = scene.AnimationKeyframeDataList[entry.Value];
                    for (int c = 0; c < keyframeData.GroupAnimationDataList[g].CastCount; ++c)
                    {
                        XNCPLib.XNCP.Animation.CastAnimationData castAnimData = keyframeData.GroupAnimationDataList[g].CastAnimationDataList[c];
                        List<AnimationTrack> tracks = new List<AnimationTrack>((int)XNCPLib.Misc.Utilities.CountSetBits(castAnimData.Flags));

                        int castAnimDataIndex = 0;
                        for (int i = 0; i < 12; ++i)
                        {
                            // check each animation type if it exists in Flags
                            if ((castAnimData.Flags & (1 << i)) != 0)
                            {
                                AnimationType type = (AnimationType)(1 << i);
                                AnimationTrack anim = new AnimationTrack(type)
                                {
                                    Field00 = castAnimData.SubDataList[castAnimDataIndex].Field00,
                                };

                                foreach (var key in castAnimData.SubDataList[castAnimDataIndex].Keyframes)
                                {
                                    anim.Keyframes.Add(new Keyframe(key));
                                }

                                tracks.Add(anim);
                                ++castAnimDataIndex;
                            }
                        }

                        if (tracks.Count > 0)
                        {
                            AnimationList layerAnimationList = new AnimationList(tempCasts[c], tracks);
                            Groups[g].Animations[entry.Key].LayerAnimations.Add(layerAnimationList);
                        }
                    }
                }

                // build hierarchy tree
                CreateHierarchyTree(g, scene.UICastGroups[g].CastHierarchyTree, tempCasts);

                tempCasts.Clear();
            }
        }

        private void ProcessSWCasts(SWScene scene, IEnumerable<TextureList> texLists, IEnumerable<UIFont> fonts)
        {
            // Create groups
            for (int g = 0; g < scene.LayerCount; ++g)
            {
                Groups.Add(new UICastGroup
                {
                    Name = scene.Layers[g].Name,
                });
            }

            // process group layers
            List<UICast> tempCasts = new List<UICast>();
            for (int g = 0; g < Groups.Count; ++g)
            {
                Dictionary<int, int> castIndexMap = new Dictionary<int, int>();
                for (int c = 0; c < scene.Layers[g].CastCellCount; ++c)
                {
                    UICast cast = new UICast(scene.Layers[g].CastNodes[c], scene.Layers[g].Cells[c],
                        scene.FrameSize, scene.Layers[g].CastNodes[c].Name, c);

                    castIndexMap.Add(scene.Layers[g].CastNodes[c].ID, c);

                    if (cast.Type == DrawType.Sprite)
                    {
                        for (int index = 0; index < scene.Layers[g].CastNodes[c].ImageCast.PatternInfoCount; ++index)
                        {
                            TextureList texList = texLists.ElementAt(scene.Layers[g].CastNodes[c].ImageCast.PatternInfoList[index].TextureListIndex);
                            Texture texture = texList.Textures.ElementAt(scene.Layers[g].CastNodes[c].ImageCast.PatternInfoList[index].TextureMapIndex);
                            cast.Sprites[index] = texture.Sprites.ElementAt(scene.Layers[g].CastNodes[c].ImageCast.PatternInfoList[index].SpriteIndex);
                        }
                    }
                    else if (cast.Type == DrawType.Font)
                    {
                        UIFont font = fonts.ElementAt((int)scene.Layers[g].CastNodes[c].ImageCast.FontInfo.FontListIndex);
                        cast.Font = font;
                    }

                    tempCasts.Add(cast);
                }

                int animations = 0;
                foreach (var animation in scene.Layers[g].Animations)
                {
                    Groups[g].Animations.Add(new AnimationGroup(animation.Name)
                    {
                        Duration = animation.FrameCount
                    });

                    foreach (var link in animation.AnimationLinks)
                    {
                        List<AnimationTrack> tracks = new List<AnimationTrack>();
                        foreach (var track in link.Tracks)
                        {
                            AnimationType type = AnimationType.None;
                            switch (track.AnimationType)
                            {
                                case 0:
                                    type = AnimationType.XPosition;
                                    break;
                                case 1:
                                    type = AnimationType.YPosition;
                                    break;
                                case 2:
                                    type = AnimationType.ZPosition;
                                    break;
                                case 6:
                                    type = AnimationType.XScale;
                                    break;
                                case 7:
                                    type = AnimationType.YScale;
                                    break;
                                case 8:
                                    type = AnimationType.ZScale;
                                    break;
                                case 17:
                                    type = AnimationType.SubImage;
                                    break;
                                case 21:
                                    type = AnimationType.ColorRed;
                                    break;
                                case 22:
                                    type = AnimationType.ColorGreen;
                                    break;
                                case 23:
                                    type = AnimationType.ColorBlue;
                                    break;
                                case 24:
                                    type = AnimationType.ColorAlpha;
                                    break;
                                default:
                                    break;
                            }

                            AnimationTrack anim = new AnimationTrack(type);
                            foreach (var key in track.Keys)
                            {
                                Keyframe keyframe = new Keyframe(key);
                                switch (type)
                                {
                                    case AnimationType.XPosition:
                                        keyframe.KValue /= scene.FrameSize.X;
                                        break;
                                    case AnimationType.YPosition:
                                        keyframe.KValue /= -scene.FrameSize.Y;
                                        break;
                                    default:
                                        break;
                                }

                                anim.Keyframes.Add(keyframe);
                                
                            }

                            tracks.Add(anim);
                        }

                        AnimationList layerAnimationList = new AnimationList(tempCasts[castIndexMap[link.CastID]], tracks);
                        Groups[g].Animations[animations].LayerAnimations.Add(layerAnimationList);
                    }

                    animations++;
                }

                // build hierarchy tree
                CreateHierarchyTree(g, scene.Layers[g].CastNodes, tempCasts);

                tempCasts.Clear();
            }
        }

        private void CreateHierarchyTree(int group, List<CastHierarchyTreeNode> tree, List<UICast> lyrs)
        {
            int next = 0;
            while (next != -1)
            {
                Groups[group].Casts.Add(lyrs[next]);
                BuildTree(next, tree, lyrs, null);

                next = tree[next].NextIndex;
            }
        }

        private void CreateHierarchyTree(int group, List<SWCastNode> casts, List<UICast> lyrs)
        {
            int next = 0;
            while (next != -1)
            {
                lyrs[next].Offset = new Vector2(0.5f, 0.5f);
                Groups[group].Casts.Add(lyrs[next]);
                BuildTree(next, casts, lyrs, null);

                next = casts[next].NextIndex;
            }
        }

        private void BuildTree(int c, List<CastHierarchyTreeNode> tree, List<UICast> lyrs, UICast parent)
        {
            int childIndex = tree[c].ChildIndex;
            if (childIndex != -1)
            {
                UICast child = lyrs[childIndex];
                lyrs[c].Children.Add(child);

                BuildTree(childIndex, tree, lyrs, lyrs[c]);
            }

            if (parent != null)
            {
                int siblingIndex = tree[c].NextIndex;
                if (siblingIndex != -1)
                {
                    UICast sibling = lyrs[siblingIndex];
                    parent.Children.Add(sibling);

                    BuildTree(siblingIndex, tree, lyrs, parent);
                }
            }
        }

        private void BuildTree(int c, List<SWCastNode> casts, List<UICast> lyrs, UICast parent)
        {
            int childIndex = casts[c].ChildIndex;
            if (childIndex != -1)
            {
                UICast child = lyrs[childIndex];
                lyrs[c].Children.Add(child);

                BuildTree(childIndex, casts, lyrs, lyrs[c]);
            }

            if (parent != null)
            {
                int siblingIndex = casts[c].NextIndex;
                if (siblingIndex != -1)
                {
                    UICast sibling = lyrs[siblingIndex];
                    parent.Children.Add(sibling);

                    BuildTree(siblingIndex, casts, lyrs, parent);
                }
            }
        }

        /// <summary>
        /// Gets the cast name from a cast dictionary provided its index and group index.
        /// If the cast is not found, an empty string is returned.
        /// </summary>
        /// <param name="groupIndex">The index of the group in which the cast belongs</param>
        /// <param name="castIndex">The index of the cast</param>
        /// <param name="castDictionary">A dictionary containing cast names, group indices and cast indices.</param>
        /// <returns></returns>
        public string GetCastName(int groupIndex, int castIndex, List<CastDictionary> castDictionary)
        {
            foreach (var entry in castDictionary)
            {
                if (entry.GroupIndex == groupIndex && entry.CastIndex == castIndex)
                    return entry.Name;
            }

            return String.Empty;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void NotifyPropertyChanged(string propertyName, object before, object after)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public int CompareTo(UIScene other)
        {
            return (int)(ZIndex - other.ZIndex);
        }
    }
}
