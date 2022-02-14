using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Collections.ObjectModel;
using XNCPLib.XNCP;
using XNCPLib.SWIF;
using Shuriken.Models;
using Shuriken.Commands;
using System.Windows;
using Shuriken.Misc;
using System.Reflection;

namespace Shuriken.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        public static string AppVersion => Assembly.GetExecutingAssembly().GetName().Version.ToString();

        public List<string> MissingTextures { get; set; }
        public ObservableCollection<ViewModelBase> Editors { get; set; }

        public MainViewModel()
        {
            MissingTextures = new List<string>();

            Editors = new ObservableCollection<ViewModelBase>
            {
                new ScenesViewModel(),
                new SpritesViewModel(),
                new FontsViewModel(),
                new AboutViewModel()
            };
#if DEBUG
            //LoadTestXNCP();
#endif
        }

        public void LoadTestXNCP()
        {
            Load("Test/ui_gameplay.xncp");
        }

        /// <summary>
        /// Loads a Ninja Chao Project file for editing
        /// </summary>
        /// <param name="filename">The path of the file to load</param>
        public void Load(string filename)
        {
            string extension = Path.GetExtension(filename);
            if (extension.ToLower() == ".xncp" || extension.ToLower() == ".yncp")
            {
                LoadXNCP(filename);
            }
            else if (extension.ToLower() == ".swif")
            {
                LoadSWIF(filename);
            }
        }

        public void LoadXNCP(string filename)
        {
            FAPCFile file = new FAPCFile();
            file.Load(filename);

            string root = Path.GetDirectoryName(Path.GetFullPath(filename));

            List<Scene> xScenes = file.Resources[0].Content.CsdmProject.Root.Scenes;
            List<SceneID> xIDs = file.Resources[0].Content.CsdmProject.Root.SceneIDTable;
            List<XTexture> xTextures = file.Resources[1].Content.TextureList.Textures;
            FontList xFontList = file.Resources[0].Content.CsdmProject.Fonts;

            Clear();

            TextureList texList = new TextureList("textures");
            foreach (XTexture texture in xTextures)
            {
                string texPath = Path.Combine(root, texture.Name);
                if (File.Exists(texPath))
                    texList.Textures.Add(new Texture(texPath));
                else
                    MissingTextures.Add(texture.Name);
            }

            if (MissingTextures.Count > 0)
                WarnMissingTextures();

            if (xScenes.Count > 0)
            {
                // Hack: we load sprites from the first scene only since whatever tool sonic team uses
                // seems to work the same way as SWIF:
                // Sprites belong to textures and layers and fonts reference a specific sprite using the texutre index and sprite index.
                foreach (SubImage subimage in xScenes[0].SubImages)
                {
                    int textureIndex = (int)subimage.TextureIndex;
                    if (textureIndex >= 0 && textureIndex < texList.Textures.Count)
                    {
                        int id = Project.CreateSprite(texList.Textures[textureIndex], subimage.TopLeft.Y, subimage.TopLeft.X,
                            subimage.BottomRight.Y, subimage.BottomRight.X);

                        texList.Textures[textureIndex].Sprites.Add(id);
                    }
                }
            }

            foreach (var entry in xFontList.FontIDTable)
            {
                UIFont font = new UIFont(entry.Name);
                foreach (var mapping in xFontList.Fonts[(int)entry.Index].CharacterMappings)
                {
                    var sprite = Utilities.FindSpriteIDFromNCPScene((int)mapping.SubImageIndex, xScenes[0].SubImages, texList.Textures);
                    font.Mappings.Add(new Models.CharacterMapping(mapping.SourceCharacter, sprite));
                }

                Project.Fonts.Add(font);
            }

            foreach (SceneID sceneID in xIDs)
                Project.Scenes.Add(new UIScene(xScenes[(int)sceneID.Index], sceneID.Name, texList, Project.Fonts));

            Project.TextureLists.Add(texList);
        }

        public void LoadSWIF(string filename)
        {
            SWIFFile file = new SWIFFile();
            file.Load(filename);

            string root = Path.GetDirectoryName(Path.GetFullPath(filename));
            List<SWScene> swScenes = file.Content.SurfWaveProject.Project.Scenes;
            List<SWTextureList> swTextureLists = file.Content.SurfWaveProject.Project.TextureLists;
            List<SWFontList> swFontLists = file.Content.SurfWaveProject.Project.FontLists;

            Clear();

            foreach (SWTextureList textureList in swTextureLists)
            {
                TextureList texList = new TextureList(textureList.Name.Value);
                foreach (SWTexture texture in textureList.Textures)
                {
                    string texPath = Path.Combine(root, texture.Name.Value + ".dds");
                    if (File.Exists(texPath))
                    {
                        Texture tex = new Texture(texPath);
                        foreach (SWSubImage subimage in texture.SubImages)
                        {
                            int id = Project.CreateSprite(tex, subimage.TopLeft.Y, subimage.TopLeft.X,
                                subimage.BottomRight.Y, subimage.BottomRight.X);
                            tex.Sprites.Add(id);
                        }

                        texList.Textures.Add(tex);
                    }
                    else
                    {
                        MissingTextures.Add(texture.Name.Value);
                    }
                }

                Project.TextureLists.Add(texList);
            }

            foreach (SWFontList fontList in swFontLists)
            {
                // Implement Texture List Index and Texture Index too here.
                UIFont font = new UIFont(fontList.Name.Value);
                for (int index = 0; index < fontList.FontMappingCount; ++index)
                {
                    TextureList texList = Project.TextureLists.ElementAt(fontList.FontMappings[index].TextureListIndex);
                    Texture texture = texList.Textures.ElementAt(fontList.FontMappings[index].TextureMapIndex);
                    var sprite = texture.Sprites.ElementAt(fontList.FontMappings[index].SpriteIndex);
                    font.Mappings.Add(new Models.CharacterMapping(Convert.ToChar(fontList.FontMappings[index].Character), sprite));
                }

                Project.Fonts.Add(font);
            }

            foreach (SWScene scene in swScenes)
            {
                Project.Scenes.Add(new UIScene(file.Content.SurfWaveProject.Project, scene, scene.Name.Value, Project.TextureLists, Project.Fonts));
            }

            if (MissingTextures.Count > 0)
                WarnMissingTextures();
        }

        public void Clear()
        {
            Project.Clear();
            MissingTextures.Clear();
        }

        private void WarnMissingTextures()
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendLine("The loaded UI file uses textures that were not found.\n");
            foreach (var texture in MissingTextures)
                builder.AppendLine(texture);

            MessageBox.Show(builder.ToString(), "Missing Textures", MessageBoxButton.OK, MessageBoxImage.Warning);
        }
    }
}
