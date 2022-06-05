

using System.IO;
using System.Net;
using System.Web.Mvc;
using System.Net.Http;

namespace ScopoERP.Web.Helper
{
    public class CommonHelper
    {
        #region FlowJS

        string root = @"C:\FTP\files";

      
        public string GetChunkFileName(int chunkNumber, string identifier)
        {
            return Path.Combine(root, string.Format("{0}_{1}", identifier, chunkNumber.ToString()));
        }

        public void RenameChunk(MultipartFileData chunk, int chunkNumber, string identifier)
        {
            string generatedFileName = chunk.LocalFileName;
            string chunkFileName = GetChunkFileName(chunkNumber, identifier);
            if (File.Exists(chunkFileName)) File.Delete(chunkFileName);
            File.Move(generatedFileName, chunkFileName);

        }

        public string GetFileName(string identifier)
        {
            return Path.Combine(root, identifier);
        }

        public bool ChunkIsHere(int chunkNumber, string identifier)
        {
            string fileName = GetChunkFileName(chunkNumber, identifier);
            return File.Exists(fileName);
        }

        public bool AllChunksAreHere(string identifier, int totalChunks)
        {
            for (int chunkNumber = 1; chunkNumber <= totalChunks; chunkNumber++)
                if (!ChunkIsHere(chunkNumber, identifier)) return false;
            return true;
        }

        public void TryAssembleFile(string identifier, int totalChunks, string filename)
        {
            if (AllChunksAreHere(identifier, totalChunks))
            {
                // Create a single file
                var consolidatedFileName = GetFileName(identifier);
                using (var destStream = File.Create(consolidatedFileName, 15000))
                {
                    for (int chunkNumber = 1; chunkNumber <= totalChunks; chunkNumber++)
                    {
                        var chunkFileName = GetChunkFileName(chunkNumber, identifier);
                        using (var sourceStream = File.OpenRead(chunkFileName))
                        {
                            sourceStream.CopyTo(destStream);
                        }
                    }
                    destStream.Close();
                }
                // Rename consolidated with original name of upload
                filename = Path.GetFileName(filename); // Strip to filename if directory is specified (avoid cross-directory attack)
                string realFileName = Path.Combine(root, filename);
                if (File.Exists(filename)) File.Delete(realFileName);
                File.Move(consolidatedFileName, realFileName);
                // Delete chunk files
                for (int chunkNumber = 1; chunkNumber <= totalChunks; chunkNumber++)
                {
                    var chunkFileName = GetChunkFileName(chunkNumber, identifier);
                    File.Delete(chunkFileName);
                }
            }
        }

        #endregion
    }
}