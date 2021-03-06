﻿using System;
using System.Windows.Input;
using Xamarin.Forms;

namespace YayMath
{
    public class MathGameViewModel : SimpleViewModel
    {
        const string selectAnswerMessage = "Select the correct answer.";
        const string correctAnswerMessage = "Correct!";
        const string incorrectAnswerMessage = "Incorrect.  Please try again.";

        MathProblemViewModel currentProblem;

        public MathGameViewModel ()
        {
            currentProblem = new MathProblemViewModel (MathProblemCreator.CreateProblem ());
            status = selectAnswerMessage;
            readyForAnswer = true;
            System.Threading.Tasks.Task.Factory.StartNew (() => DependencyService.Get<ISoundPlayer> ());
            SelectAnswer = new Command<int> ((val) => OnSelectAnswer (val).NoWarning ());
        }

        public ICommand SelectAnswer { get; private set; }

        public MathProblemViewModel CurrentProblem {
            get {
                return currentProblem;
            }
            set {
                if (currentProblem == value)
                    return;
                currentProblem = value;
                RaisePropertyChanged ();
            }
        }

        string status;
        public string Status {
            get {
                return status;
            }
            set {
                if (status == value)
                    return;
                status = value;
                RaisePropertyChanged ();
            }
        }

        bool readyForAnswer;
        public bool ReadyForAnswer {
            get {
                return readyForAnswer;
            }
            set {
                if (readyForAnswer == value)
                    return;
                readyForAnswer = value;
                RaisePropertyChanged ();
            }
        }

        async System.Threading.Tasks.Task OnSelectAnswer (int value)
        {
            if (CurrentProblem.Answer == value) {
                Status = correctAnswerMessage;
                ReadyForAnswer = false;

                DependencyService.Get<ISoundPlayer> ().PlayYay ();

                await System.Threading.Tasks.Task.Delay (1500);

                var lproblem = MathProblemCreator.CreateProblem ();
                CurrentProblem = new MathProblemViewModel (lproblem);
                Status = selectAnswerMessage;

                ReadyForAnswer = true;
            } else {
                Status = incorrectAnswerMessage;
                DependencyService.Get<ISoundPlayer> ().PlayBoo ();
            }
        }
    }
}

