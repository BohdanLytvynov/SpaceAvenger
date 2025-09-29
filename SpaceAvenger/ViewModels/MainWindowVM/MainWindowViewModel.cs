﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModelBaseLibDotNetCore.VM;
using ViewModelBaseLibDotNetCore.Commands;
using SpaceAvenger.Views.Pages;
using System.Windows.Input;
using System.Windows.Controls;
using Models.DAL.Entities.User;
using System.Reflection;
using System.Windows;
using SpaceAvenger.Enums.FrameTypes;
using SpaceAvenger.Managers.CommunicationManager;
using Microsoft.Extensions.DependencyInjection;
using SpaceAvenger.Services.Interfaces.PageManager;
using SpaceAvenger.Services.Realizations.PageManager;
using SpaceAvenger.Attributes.PageManager;
using SpaceAvenger.Services.Interfaces.MessageBus;
using SpaceAvenger.Services.Interfaces.Message;
using SpaceAvenger.Services.Realizations.Message;
using SpaceAvenger.ViewModels.Base;

namespace SpaceAvenger.ViewModels.MainWindowVM
{
    [ReflexionDetectionIgnore]
    internal class MainWindowViewModel : SubscriptableViewModel, IDisposable
    {
        #region Fields
               
        object m_mainframe;

        private object m_infoFrame;

        string m_title;

        private Guid m_userId;

        private string m_userName;

        private bool m_InfoOpenClosed;

        private GridLength m_Height;
        
        private object[] m_imagesOpenClosedButton;

        private object m_OpenClosedButton_Content;

        private IPageManagerService<FrameType>? m_pageManager;

        private IMessageBus? m_messageBus;
                
        #endregion

        #region Properties

        public object MainFrame 
        {
            get=>m_mainframe;
            set=> Set(ref m_mainframe, value);
        }

        public object InfoFrame 
        { get=> m_infoFrame; set=>Set(ref m_infoFrame, value); }

        public string Tittle 
        {
            get=> m_title;
            set=> Set(ref m_title, value);
        }

        public object OpenClosedButton_Content 
        { get=> m_OpenClosedButton_Content; set=> Set(ref m_OpenClosedButton_Content, value); }

        public GridLength Height 
        { get=> m_Height; set=> Set(ref m_Height, value); }

        #endregion

        #region Commands
        public ICommand OnOpenInfoButtonPressed { get; }
        #endregion

        #region Ctor

        public MainWindowViewModel()
        {
            m_mainframe = new object();

            m_infoFrame = new object();

            m_imagesOpenClosedButton = new object[]
                {
                    App.Current.Resources["triangleUp"],
                    App.Current.Resources["triangleDown"]
                };

            m_OpenClosedButton_Content = m_imagesOpenClosedButton[1];

            m_Height = new GridLength(0, GridUnitType.Star);

            m_title = "Space Avenger V 1.0";

            m_InfoOpenClosed = false;

            m_userName = string.Empty;

            #endregion

            #region Init Commands
            OnOpenInfoButtonPressed = new Command(
                canExecute: CanOnOpenInfoButtonPressedExecute,
                execute: OnOpenInfoButtonPressedExecute);
            #endregion


        }

        public MainWindowViewModel(IPageManagerService<FrameType> pageManager, IMessageBus msgBus) : this()
        {
            #region Init Fields

            m_messageBus = msgBus;
            
            m_pageManager = pageManager;
            
            m_pageManager.OnSwitchScreenMethodInvoked += PageManager_OnSwitchScreenMethodInvoked;

            Subscriptions.Add(m_messageBus.RegisterHandler<ChooseProfileMessage_User, User>(OnMessageRecieved));
        }

        private void OnMessageRecieved(ChooseProfileMessage_User message)
        {
            if (message.Content is not null)
                OnOpenInfoButtonPressedExecute(null);            
        }

        private void PageManager_OnSwitchScreenMethodInvoked(object? obj, PageManagerEventArgs<FrameType> args)
        {
            switch (args.FrameType)
            {
                case FrameType.MainFrame:

                    MainFrame = args.Page;
                    break;
                case FrameType.InfoFrame:

                    InfoFrame = args.Page;
                    break;                
            }
            
        }
        #endregion

        #region Methods
        
        #region On Open Info Button Pressed 
        private bool CanOnOpenInfoButtonPressedExecute(object p) => true;

        private void OnOpenInfoButtonPressedExecute(object p)
        {
            if (m_InfoOpenClosed)
            { 
                CloseInfo();

                OpenClosedButton_Content = m_imagesOpenClosedButton[1];
            }
            else
            {
                OpenInfo();

                OpenClosedButton_Content = m_imagesOpenClosedButton[0];
            }

            m_InfoOpenClosed = !m_InfoOpenClosed;
        }

        private void OpenInfo()
        {
            Task.Run(() =>
            {
                double height = 0;

                QueueWorkToDispatcher(() => height = (InfoFrame as Page)!.ActualHeight);
                
                double curr_height = 0;

                while (curr_height <= height)
                {
                    ++curr_height;

                    QueueWorkToDispatcher(() => Height = new GridLength(curr_height, GridUnitType.Star));                    
                }
            });
        }

        private void CloseInfo()
        {
            Task.Run(() =>
            {
                double curr_height = 0;

                QueueWorkToDispatcher(() => curr_height = (InfoFrame as Page)!.ActualHeight);
                
                while (curr_height > 0)
                {
                    --curr_height;
                    QueueWorkToDispatcher(() =>
                    Height = new GridLength(curr_height, GridUnitType.Star));
                }
            });
        }

        public void Dispose()
        {
            Unsubscribe();
        }

        #endregion

        #endregion
    }
}
