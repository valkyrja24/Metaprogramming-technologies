using System;

namespace Utilities
{
    public class TodoItem
    {
        private string _title;
        private bool _isDone;

        public string Title => _title;
        public bool IsDone => _isDone;

        public TodoItem(string title) : this(title, false) { }

        public TodoItem(string title, bool isDone)
        {
            if (string.IsNullOrWhiteSpace(title))
                throw new ArgumentException("Title cannot be empty or whitespace.", nameof(title));

            this._title = title;
            this._isDone = isDone;
        }

        public void MarkDone()
        {
            this._isDone = true;
        }

        public void MarkUndone()
        {
            this._isDone = false;
        }

        public bool TryRename(string newTitle)
        {
            if (string.IsNullOrWhiteSpace(newTitle))
                return false;

            this._title = newTitle;
            return true;
        }
    }
}
