using GUISharp.Components;

namespace GUISharp.Collections;

public class DepthLinkedList : LinkedList<Component>
{

    private Dictionary<int, LinkedListNode<Component>> _depthDict;

    public DepthLinkedList() : base() {
        _depthDict = new Dictionary<int, LinkedListNode<Component>>();
    }

    public void Add(Component component) {
        int depth = component.Depth;
        if(_depthDict.Count == 0) {
            var node = base.AddFirst(component);
            _depthDict[depth] = node;
            return;
        }
        _depthDict.TryGetValue(depth, out var elem);
        if(elem != null) {
            var node = base.AddAfter(elem, component);
            _depthDict[depth] = node;
        } else {
            int closestDepth = GetClosestDepth(depth);
            if(closestDepth == -1) {
                closestDepth = _depthDict.Keys.Last();
            }
            elem = _depthDict[closestDepth];
            var node = base.AddAfter(elem, component);
            _depthDict[depth] = node;
        } 
    }
    public new void Clear() {
        base.Clear();
        _depthDict.Clear();
    }

    private int GetClosestDepth(int target) {
        int closest = -1;
        foreach(var d in _depthDict.Keys) {
            closest = d;
            if(d > target) {
                break;
            }
        }
        return closest;
    }


    public new IEnumerator<Component> GetEnumerator()  // IEnumerable<int> works too.
    {
        var pivot = this.First;
        while(pivot != null) {
            yield return pivot.Value;
            pivot = pivot.Next;
        }
    }

}
