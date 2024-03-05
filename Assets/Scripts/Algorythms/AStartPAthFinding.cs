using System.Collections.Generic;
namespace Algorythms
{
    public class AStartPAthFinding
    {
            public delegate List<Tile> NeighbourStrategy(Tile from);
        public delegate float DistanceStrategy(Tile from, Tile toNeighbour);
        public delegate float HeuristicStrategy(Tile from, Tile to);

        private NeighbourStrategy _neighbours;
        private DistanceStrategy _distance;
        private HeuristicStrategy _heuristic;

        public AStartPAthFinding(NeighbourStrategy neighbours, DistanceStrategy distance, HeuristicStrategy heuristic)
        {
            _neighbours = neighbours;
            _distance = distance;
            _heuristic = heuristic;
        }

        public List<Tile> Path(Tile from, Tile to)
        {
            List<Tile> openSet = new List<Tile>() { from };

            Dictionary<Tile, Tile> cameFrom = new Dictionary<Tile, Tile>();

            Dictionary<Tile, float> gScores = new Dictionary<Tile, float>() { { from, 0f } };

            Dictionary<Tile, float> fScores = new Dictionary<Tile, float>() { { from, _heuristic(from, to) } };            

            while (openSet.Count > 0)
            {
                Tile current = FindLowestFScore(fScores, openSet);

                if (current.Equals(to))
                {
                    /*return gScores.Keys.ToList();*/ //alle nodes die g score krijgen
                    return reconstructPath(cameFrom, current); //construct path
                }              
                

                openSet.Remove(current);
                List<Tile> neighbours = _neighbours(current);

                foreach (Tile neighbour in neighbours)
                {
                    float tentativeGScore = gScores[current] + _distance(current, neighbour);
                    if (tentativeGScore < gScores.GetValueOrDefault(neighbour, float.PositiveInfinity))
                    {
                        cameFrom[neighbour] = current;
                        gScores[neighbour] = tentativeGScore;
                        fScores[neighbour] = gScores[neighbour] + _heuristic(neighbour, to);
                      ;
                        if (!openSet.Contains(neighbour))
                        {                            
                                openSet.Add(neighbour);                         
                            
                        }
                    }
                }
            }

            return new List<Tile>(0);
        }

        private List<Tile> reconstructPath(Dictionary<Tile, Tile> cameFrom, Tile current)
        {
            List<Tile> path = new List<Tile>() { current };

            while (cameFrom.ContainsKey(current))
            {
                current = cameFrom[current];
                path.Insert(0, current);
            }

            return path;
        }

        public Tile FindLowestFScore(Dictionary<Tile, float> fScores, List<Tile> openSet)
        {
            Tile currentNode = openSet[0];

            foreach (Tile node in openSet)
            {
                float currentFScore = fScores.GetValueOrDefault(currentNode, float.PositiveInfinity);
                float fscore = fScores.GetValueOrDefault(node, float.PositiveInfinity);
               
                if (fscore < currentFScore)
                {
                    currentNode = node;
                }
            }
            return currentNode;
        }
    }
    
}
