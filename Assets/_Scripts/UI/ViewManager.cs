using System.Collections.Generic;

using UnityEngine;

public class ViewManager : StaticInstance<ViewManager>
{
    [SerializeField]
    private View _startingView;

    [SerializeField]
    private List<View> _views;

    private View _currentView;

    private readonly Stack<View> _history = new Stack<View>();

    public T GetView<T>()
        where T : View
    {
        for (int i = 0; i < _views.Count; i++)
        {
            if (_views[i] is T tView)
            {
                return tView;
            }
        }

        return null;
    }

    public void Show<T>(bool remember = true)
        where T : View
    {
        for (int i = 0; i < _views.Count; i++)
        {
            if (_views[i] is T)
            {
                if (_currentView != null)
                {
                    if (remember)
                    {
                        _history.Push(_currentView);
                    }

                    _currentView.Hide();
                }

                _views[i].Show();

                _currentView = _views[i];
            }
        }
    }

    public void Show(View view, bool remember = true)
    {
        if (_currentView != null)
        {
            if (remember)
            {
                _history.Push(_currentView);
            }

            _currentView.Hide();
        }

        view.Show();

        _currentView = view;
    }

    public void ShowLast()
    {
        if (_history.Count != 0)
        {
            Show(_history.Pop(), false);
        }
    }

    private void Start()
    {
        for (int i = 0; i < _views.Count; i++)
        {
            _views[i].Init();

            _views[i].Hide();
        }

        if (_startingView != null)
        {
            Show(_startingView, true);
        }
    }
}
