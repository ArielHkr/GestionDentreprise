using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace GestionDentreprise
{
    public partial class FenetreEmploye : Window
    {
        private ListViewItem? _draggedItem;

        public FenetreEmploye()
        {
            InitializeComponent();
            MettreAJourCompteurs();

        }

        private void ListView_PreviewMouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                var listView = sender as ListView;
                if (listView == null) return;

                var item = GetListViewItemUnderMouse(listView, e.GetPosition(listView));
                if (item != null)
                {
                    _draggedItem = item;
                    DragDrop.DoDragDrop(listView, item, DragDropEffects.Move);
                }
            }
        }

        private void ListView_Drop(object sender, DragEventArgs e)
        {
            if (_draggedItem == null) return;

            var sourceList = ItemsControl.ItemsControlFromItemContainer(_draggedItem) as ListView;
            var targetList = sender as ListView;

            if (sourceList != null && targetList != null)
            {
                sourceList.Items.Remove(_draggedItem);

                targetList.Items.Add(_draggedItem);
            }

            _draggedItem = null;
            MettreAJourCompteurs();
        }

        private ListViewItem? GetListViewItemUnderMouse(ListView listView, Point position)
        {
            var element = listView.InputHitTest(position) as DependencyObject;
            while (element != null && !(element is ListViewItem))
            {
                element = System.Windows.Media.VisualTreeHelper.GetParent(element);
            }

            return element as ListViewItem;

        }
        private void MettreAJourCompteurs()
        {
            nbAFaire.Text = $"{TodoList.Items.Count}";
            nbEncours.Text = $"{DoingList.Items.Count}";
            nbFini.Text = $"{DoneList.Items.Count}";
        }

    }
}
