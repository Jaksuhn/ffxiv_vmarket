using Dalamud.Interface.Utility.Raii;
using ImGuiNET;
using Lumina.Excel.Sheets;
using System.Numerics;

namespace Market.Widget.Filters;
internal class GlamourOutfitFilter : Filter
{
    public override string Name { get; } = "Outfit";
    public override bool IsSet => showTrue == false || showFalse == false;

    private readonly List<uint> items;

    private bool showTrue = true;
    private bool showFalse = true;

    private static float _trueWidth;

    public GlamourOutfitFilter()
    {
        items = Service.DataManager.GetExcelSheet<MirageStoreSetItem>()
            .Where(x => x.RowId > 0)
            .SelectMany(x =>
            {
                var items = new List<uint>
                    {
                        x.Unknown0,
                        x.Unknown1,
                        x.Unknown2,
                        x.Unknown3,
                        x.Unknown4,
                        x.Unknown5,
                        x.Unknown6,
                        x.Unknown7,
                        x.Unknown8,
                        x.Unknown9,
                        x.Unknown10
                    }
                    .Where(y => y > 0)
                    .Select(y => y);
                return items;
            })
            .ToList();
    }

    public override bool CheckFilter(Item item) => showTrue ? items.Contains(item.RowId) : !items.Contains(item.RowId);

    public override void Draw()
    {
        using var c = ImRaii.Child($"{nameof(GlamourOutfitFilter)}-{Name}-Editor", new Vector2(-1, 24 * ImGui.GetIO().FontGlobalScale), false, ImGuiWindowFlags.None);
        if (c)
        {
            var x = ImGui.GetCursorPosX();
            if (ImGui.Checkbox("Outfit", ref showTrue))
            {
                if (!showTrue) showFalse = true;
                Modified = true;
            }

            ImGui.SameLine();

            var x2 = ImGui.GetCursorPosX() - x;
            if (x2 > _trueWidth)
                _trueWidth = x2;

            ImGui.SetCursorPosX(x + _trueWidth);
            if (ImGui.Checkbox("Not Outfit", ref showFalse))
            {
                if (!showFalse) showTrue = true;
                Modified = true;
            }
        }
    }
}
