using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// The design data source defined by this file serves as a representative example of a strongly-typed
// model used at design-time in the Visual Studio and Blend XAML editors. At run-time the 'SampleDataSource'
// is used instead.
//
// Applications may use this data source as a starting point and build on it, or discard it entirely and
// replace it with something appropriate to their needs.

namespace $safeprojectname$.Data
{
    /// <summary>
    /// Creates a collection of groups and items with design-time content.
    /// 
    /// DesignDataSource initializes with placeholder data for use at design-time.
    /// </summary>
    public sealed class DesignDataSource
    {
        public static SampleDataGroup CreateGroup(int itemCount)
        {
            SampleDataGroup group = new SampleDataGroup("Id", "Group Title", "Group Subtitle", "Assets/LightGray.png", GenerateDescription("Group Description: ", 1));

            for (int i = 0; i < itemCount; i++)
                group.Items.Add(CreateItem(group));

            return group;
        }

        public static IEnumerable<SampleDataGroup> CreateGroups()
        {
            return new SampleDataGroup[]
                    {
                        CreateGroup(5),
                        CreateGroup(7),
                        CreateGroup(3),
                        CreateGroup(6)
                    };
        }

        public static SampleDataItem CreateItem(SampleDataGroup group)
        {
            return new SampleDataItem("Id", "Item Title", "Item Subtitle", "Assets/LightGray.png", GenerateDescription("Item Description: ", 1), GenerateDescription("Item Content: ", 5), group);
        }

        public static string GenerateDescription(string title, int repeatLength)
        {
            IEnumerable<string> contentEnumerable = Enumerable.Repeat("Lorem ipsum dolor sit amet, consectetur adipiscing elit. Vivamus tempor scelerisque lorem in vehicula. Aliquam tincidunt, lacus ut sagittis tristique, turpis massa volutpat augue, eu rutrum ligula ante a ante", repeatLength);
            return title + string.Join("\n\n", contentEnumerable);
        }
    }
}
