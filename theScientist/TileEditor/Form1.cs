using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using TileEngine;
using TileEngine.Tiles;


namespace TileEditor
{
    using Image = System.Drawing.Image;

    public partial class Form1 : Form
    {

        string[] imageExtensions = new string[]
        {
            ".png",".jpg",".tga",
        };

        int maxWidth = 0, maxHeight = 0;
        
        SpriteBatch spriteBatch;
        TileLayer currentLayer;
        Texture2D emptyTile;
        Camera camera = new Camera();
        TileMap tileMap = new TileMap();

        int cellX, cellY;
        int fillCounter = 1000;

        Dictionary<string, Texture2D> textureDict = new Dictionary<string, Texture2D>();
        Dictionary<string, Image> previewDict = new Dictionary<string, Image>();
        Dictionary<string, Layer> layerDict = new Dictionary<string, Layer>();

       
       
        public GraphicsDevice GraphicsDevice
        {
            get { return tileDisplay1.GraphicsDevice; }
        }

        public Form1()
        {
                    
            InitializeComponent();

            tileDisplay1.OnInitialize += new EventHandler(tileDisplay1_OnInitialize);
            tileDisplay1.OnDraw += new EventHandler(tileDisplay1_OnDraw);

            Application.Idle += delegate { tileDisplay1.Invalidate(); };

            
            saveFileDialog1.Filter = "Layout File | *.layer";

            Mouse.WindowHandle = tileDisplay1.Handle;

            while (string.IsNullOrEmpty(txtContentPath.Text))
            {
                if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
                {
                    txtContentPath.Text = folderBrowserDialog1.SelectedPath;
                }
                else
                    MessageBox.Show("Please Chose a Content  directory");
            }

        }

       

        void tileDisplay1_OnInitialize(object sender, EventArgs e)
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            using(FileStream fileStream = new FileStream("Content/EmptyTile.png", FileMode.Open))
            {
                emptyTile = Texture2D.FromStream(GraphicsDevice,fileStream);
            }

        } 
        
        void tileDisplay1_OnDraw(object sender, EventArgs e)
        {
            Logic();
            Render();
            
        }

        public void FillCell(int x, int y, int desiredIndex)
        {
            int oldIndex = currentLayer.GetCellIndex(x, y);

            if (desiredIndex == oldIndex || fillCounter == 0)
                return;

            fillCounter--;
            currentLayer.SetCellIndex(x, y, desiredIndex);

            if (x > 0 && currentLayer.GetCellIndex(x - 1, y) == oldIndex)
                FillCell(x - 1, y, desiredIndex);
            if (x < currentLayer.Width -1 && currentLayer.GetCellIndex(x + 1, y) == oldIndex)
                FillCell(x + 1, y, desiredIndex);
            if (y > 0 && currentLayer.GetCellIndex(x, y - 1) == oldIndex)
                FillCell(x, y - 1, desiredIndex);
            if (y < currentLayer.Height -1 && currentLayer.GetCellIndex(x, y + 1) == oldIndex)
                FillCell(x, y + 1, desiredIndex);
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            if (currentLayer != null)
                currentLayer.Alpha = (float)trbAlphaSlider.Value / 100f;

        }

        private void Logic()
        {
            camera.Position.X = hScrollBar1.Value * Engine.TileWidth;
            camera.Position.Y = vScrollBar1.Value * Engine.TileHeight;

            int mx = Mouse.GetState().X;
            int my = Mouse.GetState().Y;

            if (currentLayer != null)
            {

                if (mx >= 0 && mx < tileDisplay1.Width &&
                    my >= 0 && my < tileDisplay1.Height)
                {
                    cellX = mx / Engine.TileWidth;
                    cellY = my / Engine.TileHeight;

                    cellX += hScrollBar1.Value;
                    cellY += vScrollBar1.Value;

                    cellX = (int)MathHelper.Clamp(cellX, 0, currentLayer.Width -1);
                    cellY = (int)MathHelper.Clamp(cellY, 0, currentLayer.Height -1);

                    if (Mouse.GetState().LeftButton == Microsoft.Xna.Framework.Input.ButtonState.Pressed)
                    {
                        if (rdbDraw.Checked && lstTexture.SelectedItem != null)
                        {
                            Texture2D texture = textureDict[lstTexture.SelectedItem as string];

                            int index = currentLayer.IsUsingTexture(texture);

                            if (index == -1)
                            {
                                currentLayer.AddTexture(texture);
                                index = currentLayer.IsUsingTexture(texture);
                            }

                            if (chbFill.Checked)
                            {
                                fillCounter = 1000;
                                FillCell(cellX, cellY, index);
                            }
                            else
                                currentLayer.SetCellIndex(cellX, cellY, index);
                        }

                        else if (rdbErase.Checked)
                        {
                            if (chbFill.Checked)
                            {
                                fillCounter = 1000;
                                FillCell(cellX, cellY, -1);
                            }
                            else
                                currentLayer.SetCellIndex(cellX, cellY, -1);
                        }
                    }
                }

                else
                {
                    cellX = cellY = -1;
                }
            }
        }

        private void Render()
        {
            GraphicsDevice.Clear(Microsoft.Xna.Framework.Color.Black);

            foreach (var layer in tileMap.Layers)
            {
                layer.Draw(spriteBatch, camera);

                spriteBatch.Begin();
                for (int y = 0; y < layer.Height; y++)
                {
                    for (int x = 0; x < layer.Width; x++)
                    {
                        if (layer.GetCellIndex(x, y) == -1)
                        {
                            spriteBatch.Draw(
                                emptyTile,
                                new Rectangle(
                                    x * Engine.TileWidth - (int)camera.Position.X,
                                    y * Engine.TileHeight - (int)camera.Position.Y,
                                    Engine.TileWidth,
                                    Engine.TileHeight),
                                Color.White);
                        }
                    }
                }
                spriteBatch.End();

                if (layer == currentLayer)
                    break;
            }

            if (currentLayer != null)
            {

                if (cellX != -1 && cellY != -1)
                {
                   spriteBatch.Begin();
                   spriteBatch.Draw(
                                emptyTile,
                                new Rectangle(
                                    cellX * Engine.TileWidth - (int)camera.Position.X,
                                    cellY * Engine.TileHeight - (int)camera.Position.Y,
                                    Engine.TileWidth,
                                    Engine.TileHeight),
                                Color.Red);    

                   spriteBatch.End();
                }
            }
        }


        private void newTileMapToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openFileDialog1.Filter = "Layout File | *.layer";
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string filename = openFileDialog1.FileName;

                string[] textureNames;

                TileLayer layer = TileLayer.FromFile(filename, out textureNames);

                

                layerDict.Add(Path.GetFileName(filename), layer);
                tileMap.Layers.Add(layer);
                lstLayers.Items.Add(Path.GetFileName(filename));

                foreach (string textureName in textureNames)
                {                   
                    if (textureDict.ContainsKey(textureName))
                    {
                        layer.AddTexture(textureDict[textureName]);
                        continue;
                    }

                    string fullPath = txtContentPath.Text + "/" + textureName;

                    foreach (string ext in imageExtensions)
                    {
                        if (File.Exists(fullPath + ext))
                        {
                            fullPath += ext;
                            break;
                        }
                    }

                    Texture2D tex;
                    using (FileStream fileStream = new FileStream(fullPath, FileMode.Open))
                    {
                        tex = Texture2D.FromStream(GraphicsDevice, fileStream);
                    }
                  
                    Image image = Image.FromFile(fullPath);
                    textureDict.Add(textureName, tex);
                    previewDict.Add(textureName, image);

                    lstTexture.Items.Add(textureName);
                    layer.AddTexture(tex);
                }

                AdjustScrollBars();
            }
        }

        private void AdjustScrollBars()
        {
            if (tileMap.GetWidthInPixels() > tileDisplay1.Width)
            {
                maxWidth = (int)Math.Max(tileMap.GetWidth(), maxWidth);

                hScrollBar1.Visible = true;
                hScrollBar1.Minimum = 0;
                hScrollBar1.Maximum = maxWidth;
            }
            else
            {
                maxWidth = 0;
                hScrollBar1.Visible = false;
            }

            if (tileMap.GetHeightInPixels() > tileDisplay1.Height)
            {
                maxHeight = (int)Math.Max(tileMap.GetHeight(), maxHeight);

                vScrollBar1.Visible = true;
                vScrollBar1.Minimum = 0;
                vScrollBar1.Maximum = maxHeight;
            }
            else
            {
                maxHeight = 0;
                vScrollBar1.Visible = false;
            }
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (var item in lstLayers.Items)
            {
                string filename = item as string;
                saveFileDialog1.FileName = filename;

                if (filename.Contains("Collision"))
                {
                    CollisionLayer collisionLayer = (CollisionLayer)layerDict[filename];

                    if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                    {
                        collisionLayer.SaveLayerToFile(saveFileDialog1.FileName, null);
                    }    
                
                }
                else
                {

                    TileLayer tileLayer = (TileLayer)layerDict[filename];

                    Dictionary<int, string> utilizedTextures = new Dictionary<int, string>();
                    foreach (string textureName in lstTexture.Items)
                    {
                        int index = tileLayer.IsUsingTexture(textureDict[textureName]);

                        if (index != -1)
                        {
                            utilizedTextures.Add(index, textureName);
                        }
                    }

                    List<string> utilizedTextureList = new List<string>();

                    for (int i = 0; i < utilizedTextures.Count; i++)
                    {
                        utilizedTextureList.Add(utilizedTextures[i]);
                    }

                    if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                    {
                        tileLayer.SaveLayerToFile(saveFileDialog1.FileName, utilizedTextureList.ToArray());
                    }

                }
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void lstTexture_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lstTexture.SelectedItem != null)
            {
                picbTexturePreviev.Image = previewDict[lstTexture.SelectedItem as string];
            }
        }

        private void lstLayers_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lstLayers.SelectedItem != null)
                currentLayer = (TileLayer)layerDict[lstLayers.SelectedItem as string];
                trbAlphaSlider.Value = (int)currentLayer.Alpha * 100;
        }

        private void btnAddLayer_Click(object sender, EventArgs e)
        {
            frmNewLayer newLayerForm = new frmNewLayer();
            newLayerForm.ShowDialog();

            if (newLayerForm.OKPressed)
            {
                if (lstLayers.Items.Count != 0)
                {
                    TileLayer tileLayer = new TileLayer(
                       int.Parse(newLayerForm.txtLayerHeight.Text),
                       int.Parse(newLayerForm.txtLayerWidth.Text));

                    layerDict.Add(newLayerForm.txtLayerName.Text, tileLayer);
                    tileMap.Layers.Add(tileLayer);
                    lstLayers.Items.Add(newLayerForm.txtLayerName.Text);

                    AdjustScrollBars();
                }

                else
                {
                    TileLayer tileLayer = new TileLayer(
                       int.Parse(newLayerForm.txtLayerHeight.Text),
                       int.Parse(newLayerForm.txtLayerWidth.Text));

                    CollisionLayer collisionLayer = new CollisionLayer(
                       int.Parse(newLayerForm.txtLayerHeight.Text),
                       int.Parse(newLayerForm.txtLayerWidth.Text));
                    
                    layerDict.Add(newLayerForm.txtLayerName.Text, tileLayer);
                    tileMap.Layers.Add(tileLayer);
                    lstLayers.Items.Add(newLayerForm.txtLayerName.Text);

                    layerDict.Add(newLayerForm.txtLayerName.Text + "Collision", collisionLayer);
                    tileMap.CollisionLayer = collisionLayer;
                    lstLayers.Items.Add(newLayerForm.txtLayerName.Text+"Collision");

                    AdjustScrollBars();
                }
            }
        }

        private void btnRemoveLayer_Click(object sender, EventArgs e)
        {
            if (currentLayer != null)
            {
                string filename = lstLayers.SelectedItem as string;
                
                tileMap.Layers.Remove(currentLayer);
                layerDict.Remove(filename);
                lstLayers.Items.Remove(lstLayers.SelectedItem);

                currentLayer = null;

                AdjustScrollBars();
            }
        }

        private void btnAddTexture_Click(object sender, EventArgs e)
        {
            openFileDialog1.Filter = "JPG Image| *.jpg|PNG Image| *.png|TGA Image| *.tga";
            openFileDialog1.InitialDirectory = txtContentPath.Text;

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string filename = openFileDialog1.FileName;

                using (FileStream fileStream = new FileStream(filename, FileMode.Open))
                {
                    Texture2D texture = Texture2D.FromStream(GraphicsDevice, fileStream);

                    Image image = Image.FromStream(fileStream);

                    filename = filename.Replace(txtContentPath.Text + "\\", "");
                    filename = filename.Remove(filename.LastIndexOf("."));

                    lstTexture.Items.Add(filename);
                    textureDict.Add(filename, texture);
                    previewDict.Add(filename, image);
                }

            }
        }

        private void btnRemoveTexture_Click(object sender, EventArgs e)
        {
            if (lstTexture != null)
            {
                string textureName = lstTexture.SelectedItem as string;

                foreach (TileLayer layer in tileMap.Layers)
                {
                    if (layer.IsUsingTexture(textureDict[textureName]) != -1)
                    {
                        layer.RemoveTexture(textureDict[textureName]);
                    }

                    textureDict.Remove(textureName);
                    previewDict.Remove(textureName);
                    lstTexture.Items.Remove(lstTexture.SelectedItem);

                    picbTexturePreviev.Image = null;

                }
            }
        }

       
      
    }
}
