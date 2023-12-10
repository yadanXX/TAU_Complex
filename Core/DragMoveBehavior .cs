using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Interactivity;
using System.Windows.Input;
using System.Windows.Shapes;

namespace TAU_Complex.Core
{
    public class DragMoveBehavior : Behavior<Rectangle>
    {

        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.MouseMove += AssociatedObject_MouseMove;
        }

        private void AssociatedObject_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed && sender is Rectangle rectangle)
            {
                (rectangle.Parent as Window).DragMove();
            }
        }
    }
}
