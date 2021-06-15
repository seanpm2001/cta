﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using NUnit.Framework;
using CTA.WebForms2Blazor.Factories;
using CTA.WebForms2Blazor.Services;

namespace CTA.WebForms2Blazor.Tests.Factories
{
    [TestFixture]
    class FileInformationFactoryTests
    {
        private const string TestFilesDirectoryPath = "TestingArea/TestFiles";

        private string _testProjectPath;
        private string _testCodeFilePath;
        private string _testConfigFilePath;
        private string _testStaticFilePath;
        private string _testViewFilePath;
        private string _testProjectFilePath;

        private FileInformationFactory _fileFactory;

        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            var workingDirectory = Environment.CurrentDirectory;
            _testProjectPath = Directory.GetParent(workingDirectory).Parent.Parent.FullName;
            var testFilesPath = Path.Combine(_testProjectPath, TestFilesDirectoryPath);
            _testCodeFilePath = Path.Combine(testFilesPath, "TestClassFile.cs");
            _testConfigFilePath = Path.Combine(testFilesPath, "SampleConfigFile.config");
            _testStaticFilePath = Path.Combine(testFilesPath, "SampleStaticFile.png");
            _testViewFilePath = Path.Combine(testFilesPath, "SampleViewFile.aspx");
            _testProjectFilePath = Path.Combine(testFilesPath, "SampleProjectFile.csproj");
        }

        [SetUp]
        public void Setup()
        {
            var webFormsWorkspaceManager = new WorkspaceManagerService();
            var blazorWorkspaceManager = new WorkspaceManagerService();

            blazorWorkspaceManager.CreateSolutionFile();
            webFormsWorkspaceManager.CreateSolutionFile();

            _fileFactory = new FileInformationFactory(_testProjectPath, blazorWorkspaceManager, webFormsWorkspaceManager);
        }

        [Test]
        public void TestBuildBasic()
        {
            FileInformationModel.FileConverter codeFileObj = _fileFactory.Build(new FileInfo(_testCodeFilePath));
            FileInformationModel.FileConverter configFileObj = _fileFactory.Build(new FileInfo(_testConfigFilePath));
            FileInformationModel.FileConverter staticFileObj = _fileFactory.Build(new FileInfo(_testStaticFilePath));
            FileInformationModel.FileConverter viewFileObj = _fileFactory.Build(new FileInfo(_testViewFilePath));
            FileInformationModel.FileConverter projectFileObj = _fileFactory.Build(new FileInfo(_testProjectFilePath));

            Assert.True(typeof(FileInformationModel.CodeFileConverter).IsInstanceOfType(codeFileObj));
            Assert.True(typeof(FileInformationModel.ConfigFileConverter).IsInstanceOfType(configFileObj));
            Assert.True(typeof(FileInformationModel.StaticFileConverter).IsInstanceOfType(staticFileObj));
            Assert.True(typeof(FileInformationModel.ViewFileConverter).IsInstanceOfType(viewFileObj));
            Assert.True(typeof(FileInformationModel.ProjectFileConverter).IsInstanceOfType(projectFileObj));

        }

        [Test]
        public void TestBuildManyBasic()
        {
            List<FileInfo> files = new List<FileInfo>();
            files.Add(new FileInfo(_testCodeFilePath));
            files.Add(new FileInfo(_testConfigFilePath));
            files.Add(new FileInfo(_testStaticFilePath));
            files.Add(new FileInfo(_testViewFilePath));
            files.Add(new FileInfo(_testProjectFilePath));

            List<FileInformationModel.FileConverter> fileObjects = _fileFactory.BuildMany(files).ToList();
            Assert.True(typeof(FileInformationModel.CodeFileConverter).IsInstanceOfType(fileObjects[0]));
            Assert.True(typeof(FileInformationModel.ConfigFileConverter).IsInstanceOfType(fileObjects[1]));
            Assert.True(typeof(FileInformationModel.StaticFileConverter).IsInstanceOfType(fileObjects[2]));
            Assert.True(typeof(FileInformationModel.ViewFileConverter).IsInstanceOfType(fileObjects[3]));
            Assert.True(typeof(FileInformationModel.ProjectFileConverter).IsInstanceOfType(fileObjects[4]));
        }
    }
}
