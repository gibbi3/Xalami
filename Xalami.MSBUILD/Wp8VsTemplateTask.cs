﻿using FutuFormTemplate.MSBUILD;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Xalami.MSBUILD
{
    public class Wp8VsTemplateTask : XalamiTask
    {
        public override bool Run(string csprojPath, string targetDir, string projectFriendlyName, string previewImagePath)
        {
            CsprojFile = csprojPath;
            ProjectFriendlyName = projectFriendlyName;            
            PreviewImagePath = previewImagePath;

            tempFolder = Path.Combine(targetDir, Constants.TEMPFOLDER, Constants.WP8PLATFORMSUFFIX);
            if (Directory.Exists(tempFolder))
            {
                Directory.Delete(tempFolder, true);
            }

            string projectFolder = Path.GetDirectoryName(CsprojFile);
            CopyProjectFilesToTempFolder(projectFolder, tempFolder);

            ReplaceNamespace(tempFolder);
            FileHelper.DeleteKey(tempFolder);
            ProcessVSTemplate(tempFolder);
            OperateOnCsProj(tempFolder, CsprojFile, Constants.WP8PLATFORMSUFFIX);
            OperateOnManifest(Path.Combine(tempFolder, "Package.appxmanifest"));              

            return true;
        }

        /// <summary>
        /// Operates the on manifest.
        /// </summary>
        /// <param name="manifestFile">The manifest file.</param>
        private void OperateOnManifest(string manifestFile)
        {
            string manifestText = FileHelper.ReadFile(manifestFile);

            var replacements = new List<FindReplaceItem>();

            replacements.Add(new FindReplaceItem() { Pattern = "<mp:PhoneIdentity(.*?)/>", Replacement = @"<mp:PhoneIdentity PhoneProductId=""$$guid9$$"" PhonePublisherId=""00000000-0000-0000-0000-000000000000""/>" });
            replacements.Add(new FindReplaceItem() { Pattern = "<DisplayName>(.*?)</DisplayName>", Replacement = @"<DisplayName>$$projectname$$</DisplayName>" });
            replacements.Add(new FindReplaceItem() { Pattern = "<PublisherDisplayName>(.*?)</PublisherDisplayName>", Replacement = @"<PublisherDisplayName>$$XmlEscapedPublisher$$</PublisherDisplayName>" });
            replacements.Add(new FindReplaceItem() { Pattern = @"Executable=""(.*?)""", Replacement = @"Executable=""$$targetnametoken$$.exe""" });
            replacements.Add(new FindReplaceItem() { Pattern = @"EntryPoint=""(.*?)""", Replacement = @"EntryPoint=""$$ext_safeprojectname$$.App""" });
            replacements.Add(new FindReplaceItem() { Pattern = @"DisplayName=""(.*?)""", Replacement = @"DisplayName=""$$ext_projectname$$.App""" });
            replacements.Add(new FindReplaceItem() { Pattern = @"EntryPoint=""(.*?)""", Replacement = @"EntryPoint=""$$ext_projectname$$.App""" });

            foreach (var item in replacements)
            {
                manifestText = Regex.Replace(manifestText, item.Pattern, item.Replacement);
            }

            manifestText = ReplaceIdentityNode(manifestText);

            FileHelper.WriteFile(manifestFile, manifestText);
        }        

        /// <summary>
        /// Replaces the identity node.
        /// </summary>
        /// <param name="manifestText">The manifest text.</param>
        /// <returns></returns>
        private string ReplaceIdentityNode(string manifestText)
        {
            string findText = @"<Identity";
            if (!manifestText.Contains(findText))
            {
                return manifestText;
            }

            string identityReplacementText = @"<Identity
    Name=""$guid9$""
    Publisher = ""$XmlEscapedPublisherDistinguishedName$""
    Version = ""1.0.0.0"" /> ";

            int findTextIndex, start, end;
            string firstHalf, lastHalf;

            findTextIndex = manifestText.IndexOf(findText);

            start = findTextIndex;
            end = manifestText.IndexOf("/>", findTextIndex);
            firstHalf = manifestText.Substring(0, start);
            lastHalf = manifestText.Substring(end + 2);
            return firstHalf + identityReplacementText + lastHalf;
        }

        protected override void ProcessVSTemplate(string tempFolder)
        {
            string xml = FileHelper.ReadFile(CsprojFile);
            string projectName = Path.GetFileName(CsprojFile);
            string projXml = GetProjectNode(xml, projectName);
            xml = Constants.WP8TEMPLATETEXT.Replace(Constants.PROJECTNODE, projXml);
            xml = xml.Replace(Constants.TEMPLATENAME, ProjectFriendlyName);            

            string filePath = Path.Combine(tempFolder, Constants.WP8TEMPLATENAME);

            FileHelper.WriteFile(filePath, xml);
        }


    }
}
