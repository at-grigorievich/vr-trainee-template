using System;
using System.Collections.Generic;
using UnityEngine;

namespace ATG.Command
{
    public sealed class CommandInvoker: ICommand
    {
        private readonly ICommand[] _commands;
        private readonly bool _ignoreIfAlreadyExecuted;
        
        private Queue<ICommand> _commandQueue;
        private ICommand _currentCommand;
        
        public event Action<bool> OnCompleted;
        
        public CommandInvoker(bool ignoreIfAlreadyExecuted = false, params ICommand[] commands)
        {
            _ignoreIfAlreadyExecuted = ignoreIfAlreadyExecuted;
            _commands = commands;
        }

        public void Execute()
        {
            if (_ignoreIfAlreadyExecuted == false && _currentCommand != null)
            {
                Debug.LogWarning("Command already executed");
                return;
            }
            
            Reset();
            
            _currentCommand = _commandQueue.Dequeue();
            _currentCommand.OnCompleted += MoveNext;
            _currentCommand.Execute();
        }

        public void Dispose()
        {
            foreach (var command in _commands)
            {
                command.Dispose();
            }
            
            _commandQueue.Clear();
        }
        
        private void Reset()
        {
            _currentCommand = null;
            _commandQueue = new Queue<ICommand>(_commands);
        }

        private void MoveNext(bool commandExecutedStatus)
        {
            _currentCommand.OnCompleted -= MoveNext;
            _currentCommand = null;
            
            if (commandExecutedStatus == false)
            {
                OnCompleted?.Invoke(false);
                return;
            }

            if (_commandQueue.Count == 0)
            {
                OnCompleted?.Invoke(true);
                return;
            }
            
            _currentCommand = _commandQueue.Dequeue();
            _currentCommand.OnCompleted += MoveNext;
            _currentCommand.Execute();
        }
    }
}