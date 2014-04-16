using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content.Pipeline;
using Microsoft.Xna.Framework.Content.Pipeline.Graphics;
using System.Xml;


namespace TileContent
{
    /// <summary>
    /// Läser in en .dialog fil och sparar den som en XML fil
    /// Sedan skickas filen till ScriptProcessor klassen
    /// </summary>
    [ContentImporter(".dialog", DisplayName = "NPC script Importer", DefaultProcessor = "ScriptProcessor")]
    public class ScriptImporter : ContentImporter<XmlDocument>
    {
        public override XmlDocument Import(string filename, ContentImporterContext context)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(filename);
            return doc;
        }
    }
}
