using System.Collections.Generic;
using BehaviorTree;

public class AIBlue : Tree
{
    private APIPlayer _api;
    protected override BehaviorTree.Node CreateTree()
    {
        _api = GetComponent<APIPlayer>();
        BehaviorTree.Node root = new Selector(new List<BehaviorTree.Node>
        {
            new UseItemRandomly(_api),
            new SleepUntil(_api),
            new Sequence(new List<BehaviorTree.Node>
            {
                new CheckMinotaurNear(_api),
                new RunAwayFromMinotaur(_api),
            }),
            new Sequence(new List<BehaviorTree.Node>
            {
                new CheckPortalNear(_api),
                new GoToTarget(_api),
                new EnterPortal(_api),
            }),
            new Sequence(new List<BehaviorTree.Node>
            {
                new CheckItemsNear(_api),
                new CheckItemSlotAvailable(_api),
                new GoToTarget(_api),
                new PickUpItem(_api),
            }),
            new Sequence(new List<BehaviorTree.Node>
            {
                new CheckHaveOrb(_api),
                new BringOrbBack(_api),
                new PutOrb(_api),
            }),
            new Sequence(new List<BehaviorTree.Node>
            {
                new CheckOrbNear(_api),
                new GoToTarget(_api),
                new PickUpOrb(_api),
            }),
            new Sequence(new List<BehaviorTree.Node>
            {
                new CheckOptionalWallNear(_api),
                new CheckCanPlaceWall(_api),
                new GoToTarget(_api),
                new PlaceOptionalWall(_api),
            }),
            new DepthFirstExploration(_api),
        });
        return root;
    }
}