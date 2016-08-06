using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace GitSubmodules.Helper
{
    /// <summary>
    /// Helper class to easier use the <see cref="INotifyPropertyChanged"/> event
    /// </summary>
    public abstract class ModelBase : INotifyPropertyChanged
    {
        #region INotifyPropertyChanged Member

        /// <summary>
        /// The <see cref="PropertyChangedEventHandler"/> that can start the surface update of a element
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        #endregion INotifyPropertyChanged Member

        #region Internal Methods

        /// <summary>
        /// Call the <see cref="PropertyChangedEventHandler"/> event to update a surface element
        /// </summary>
        /// <param name="propertyName">The property that should be update on the surface</param>
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            if ((PropertyChanged == null) || string.IsNullOrEmpty(propertyName))
            {
                return;
            }

            PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion Internal Methods
    }
}
