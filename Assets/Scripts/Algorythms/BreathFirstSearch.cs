using System.Collections.Generic;
namespace Algorythms
{
    public class BreathFirstSearch
    {
        public delegate List<Tile> NeighbourStrategy(Tile from);
        public delegate float DistanceStrategy(Tile from, Tile to);

        private NeighbourStrategy _neighbours;
        private DistanceStrategy _distance;

        public BreathFirstSearch(NeighbourStrategy neighbours/*, DistanceStrategy distance*/)
        {
            _neighbours = neighbours;
            //_distance = distance;
        }

        public List<Tile> Area(Tile centerPosition, float maxDistance)
        {
            int level = 0;

            List<Tile> nearbyPosition = new List<Tile>();
            nearbyPosition.Add(centerPosition);

            Queue<Tile> nodesToVisit = new Queue<Tile>();
            nodesToVisit.Enqueue(centerPosition);
            nodesToVisit.Enqueue(default(Tile));


            while (nodesToVisit.Count > 0 && level < maxDistance)
            {
                Tile currentNode = nodesToVisit.Dequeue();

                if (currentNode == null)
                {
                    level++;
                    nodesToVisit.Enqueue(default(Tile));

                    if (nodesToVisit.Peek() == null)
                        break;
                    else
                        continue;
                }

                List<Tile> neighbours = _neighbours(currentNode);
                foreach (Tile neighbour in neighbours)
                {                    
                    if (nearbyPosition.Contains(neighbour))
                    {
                        continue;
                    }

                    nearbyPosition.Add(neighbour);
                    nodesToVisit.Enqueue(neighbour);
                }

            }

            nearbyPosition.Remove(centerPosition);
            return nearbyPosition;
        }
    }
}
