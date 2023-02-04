from __future__ import annotations

import random
import matplotlib.pyplot as plt
from typing import Optional, Iterator
import numpy as np
from matplotlib import colors
import bisect

random.seed(48542)


class Node:
    def __init__(self, name: Optional[str] = None):
        self.name: Optional[str] = name
        self._tags: set[str] = set()

    def __repr__(self):
        return f'<Node: {self.name}>' if self.name else '<Node>'

    def add_tag(self, tag: str):
        self._tags.add(tag)

    def remove_tag(self, tag: str):
        self._tags.remove(tag)

    def has_tag(self, tag: str) -> bool:
        return tag in self._tags

    @property
    def tags(self) -> set[str]:
        return self._tags.copy()


class Graph:
    def __init__(self, *args: Node):
        self._nodes: list[Node] = []
        self._edges: dict[tuple[Node, Node], float] = {}
        self.bulk_add_node(*args)

    def add_node(self, node: Node):
        if not isinstance(node, Node):
            raise TypeError("Argument must be of type Node")
        if node not in self._nodes:
            self._nodes.append(node)

    def remove_node(self, node: Node):
        if not isinstance(node, Node):
            raise TypeError("Argument must be of type Node")
        if node in self._nodes:
            self._nodes.remove(node)

    def bulk_add_node(self, *args: Node):
        for i in args:
            self.add_node(i)

    def bulk_remove_node(self, *args: Node):
        for i in args:
            self.remove_node(i)

    def is_node(self, node: Node) -> bool:
        return node in self._nodes

    def is_edge(self, node1: Node, node2: Node) -> bool:
        return self.edge_weight(node1, node2) is not None

    def edge_weight(self, node1: Node, node2: Node) -> Optional[float]:
        if not (isinstance(node1, Node) and isinstance(node2, Node)):
            raise TypeError("Arguments must be of type Node")
        if (node1, node2) in self._edges:
            return self._edges[(node1, node2)]
        elif (node2, node1) in self._edges:
            return self._edges[(node2, node1)]
        else:
            return None

    def set_weight(self, node1: Node, node2: Node, weight: float):
        if not (isinstance(node1, Node) and isinstance(node2, Node)):
            raise TypeError("Arguments must be of type Node")
        if not (node1 in self._nodes and node2 in self._nodes):
            raise ValueError("Both nodes must be in the graph")
        if (node2, node1) in self._edges:
            self._edges[(node2, node1)] = weight
        else:
            self._edges[(node1, node2)] = weight

    def add_edge(self, node1: Node, node2: Node, weight: float = 1):
        if not (isinstance(node1, Node) and isinstance(node2, Node)):
            raise TypeError("Arguments must be of type Node")
        if not (node1 in self._nodes and node2 in self._nodes):
            raise ValueError("Both nodes must be in the graph")
        self._edges[(node1, node2)] = weight

    def remove_edge(self, node1: Node, node2: Node):
        if not (isinstance(node1, Node) and isinstance(node2, Node)):
            raise TypeError("Arguments must be of type Node")
        if (node1, node2) in self._edges:
            del self._edges[(node1, node2)]
        elif (node2, node1) in self._edges:
            del self._edges[(node2, node1)]
        else:
            raise ValueError("Edge does not exist")

    def distance(self, node1: Node, node2: Node):
        if not (isinstance(node1, Node) and isinstance(node2, Node)):
            raise TypeError("Arguments must be of type Node")
        if not (node1 in self._nodes and node2 in self._nodes):
            raise ValueError("Both nodes must be in the graph")
        if node1 == node2:
            return 0
        dist: dict[Node, int] = {}
        dist[node1] = 0
        for node in self._nodes:
            if node != node1:
                dist[node] = float('inf')
        queue = [node1]
        while queue:
            node = queue.pop(0)
            for adj in self.adjacent(node):
                if dist[adj] == float('inf'):
                    dist[adj] = dist[node] + 1
                    queue.append(adj)
        return dist[node2]

    def clear_edges(self):
        self._edges.clear()

    def adjacent(self, node: Node):
        return [n for n in self._nodes if self.is_edge(node, n)]

    def clone(self) -> Graph:
        result = Graph()
        result._nodes = self._nodes.copy()
        result._edges = self._edges.copy()
        return result

    def prim_algorithm(self) -> Graph:
        tree: Graph = self.clone()
        tree.clear_edges()
        inf: float = float("inf")
        visited: set[Node] = set()
        father: dict[Node, Node] = {}
        unseen: dict[Node, float] = {}
        for node in self._nodes:
            unseen[node] = inf
            father[node] = node
        s = self._nodes[0]
        unseen[s] = 0
        while len(self._nodes) > len(visited):
            u = min(unseen, key=unseen.get)
            tree.add_edge(u, father[u], 1)
            visited.add(u)
            for v in self.adjacent(u):
                if v not in visited and unseen[v] > self.edge_weight(u, v):
                    unseen[v] = self.edge_weight(u, v)
                    father[v] = u
            del unseen[u]
        return tree

    # def prim_algorithm(self):
    #     tree: Graph = self.clone()
    #     tree.clear_edges()
    #     inf: float = float("inf")
    #     memory: dict[Node, tuple[float, Optional[Node]]] = {node: (inf, None) for node in self._nodes}
    #     seen = set()
    #     s = self._nodes[0]
    #     seen.add(s)
    #     while len(self._nodes) > len(seen):
    #         neighbors = self.adjacent(s)
    #         for neighbor in neighbors:
    #             if neighbor not in seen:
    #                 weight = self.edge_weight(s, neighbor)
    #                 if weight < memory[neighbor][0]:
    #                     memory[neighbor] = (weight, s)
    #         s = min([y for x in seen for y in self.adjacent(x) if y not in seen], key=lambda x: memory[x][0])
    #         seen.add(s)
    #         tree.add_edge(s, memory[s][1], 1)
    #     return tree

    @property
    def nodes(self) -> Iterator[Node]:
        return iter(self._nodes)

    @property
    def edges(self) -> Iterator[tuple[Node, Node]]:
        return iter(self._edges)


class Grid(Graph):
    def __init__(self, height: int, width: int):
        super().__init__()
        self._width: int = width
        self._height: int = height
        self._nodes: list[Node] = [Node(f'{i},{j}') for i in range(width) for j in range(height)]
        for i in range(height):
            for j in range(width):
                if i > 0:
                    self.add_edge(self[i, j], self[i - 1, j])
                if j > 0:
                    self.add_edge(self[i, j], self[i, j - 1])

    @property
    def width(self) -> int:
        return self._width

    @property
    def height(self) -> int:
        return self._height

    def __getitem__(self, key: tuple[int, int]) -> Node:
        a, b = key
        if a < 0:
            a = self._height + a
        if b < 0:
            b = self._width + b
        return self._nodes[a * self._width + b]

    def clone(self) -> Graph:
        result = Grid(self._height, self._width)
        result._edges = self._edges.copy()
        result._nodes = self._nodes.copy()
        return result

    def distance(self, node1: Node, node2: Node) -> int:
        """Apply a star algorithm to find the distance between two nodes"""
        if not (isinstance(node1, Node) and isinstance(node2, Node)):
            raise TypeError("Arguments must be of type Node")
        if not (node1 in self._nodes and node2 in self._nodes):
            raise ValueError("Both nodes must be in the graph")
        if node1 == node2:
            return 0
        start = [None, node1]
        end = [None, node2]
        open_list = [start]
        closed_list = []
        while open_list:
            current = open_list.pop(0)
            closed_list.append(current)
            if current[1] == end[1]:
                break
            for neighbor in self.adjacent(current[1]):
                if neighbor in [x[1] for x in closed_list]:
                    continue
                if neighbor not in [x[1] for x in open_list]:
                    open_list.append([current[1], neighbor])
        path = []
        while current[1] != start[1]:
            path.append(current[1])
            for node in closed_list:
                if node[1] == current[0]:
                    current = node
                    break
        return len(path)


    def show(self):
        plt.figure(figsize=(self._width, self._height))
        plt.plot([0, self._width], [0, 0], "k")
        plt.plot([0, self._width], [self._height, self._height], "k")
        plt.plot([0, 0], [0, self._height], "k")
        plt.plot([self._width, self._width], [0, self._height], "k")
        for i in range(self._height):
            for j in range(self._width):
                if i > 0:
                    if not self.is_edge(self[i, j], self[i - 1, j]):
                        plt.plot([j, j + 1], [i, i], color="black")
                if j > 0:
                    if not self.is_edge(self[i, j], self[i, j - 1]):
                        plt.plot([j, j], [i, i + 1], color="black")
        plt.axis("off")
        plt.show()

    def to_matrix(self) -> np.array:
        matrix: np.array = np.full((self._height*2-1, self._width*2-1), 1)
        for i in range(self._height*2-1):
            for j in range(self._width*2-1):
                if i % 2 == 1 and not self.is_edge(self[i//2, j//2], self[i//2+1, j//2]):
                    matrix[i, j] = 0
                elif j % 2 == 1 and not self.is_edge(self[i//2, j//2], self[i//2, j//2+1]):
                    matrix[i, j] = 0
        for i in range(self._height*2-1):
            for j in range(self._width*2-1):
                if i % 2 == 1 and j % 2 == 1 and \
                        any(matrix[x, y] == 0 for x in range(i-1, i+2) for y in range(j-1, j+2)):
                    matrix[i, j] = 0
                elif (self[i//2, j//2].has_tag("center") or self[(i+1)//2, (j+1)//2].has_tag("center")) \
                        and matrix[i, j] == 1:
                    matrix[i, j] = 2
        matrix = np.vstack((np.zeros((1, self._width*2-1)), matrix, np.zeros((1, self._width*2-1))))
        matrix = np.hstack((np.zeros((self._height*2+1, 1)), matrix, np.zeros((self._height*2+1, 1))))
        return matrix

    @classmethod
    def make_labyrinth(cls, height: int, width: int, center_radius: int = 0) -> Grid:
        grid = cls(height, width)
        center_x: int = width // 2
        center_y: int = height // 2
        if center_radius:
            if height % 2 == 0 or width % 2 == 0:
                raise ValueError("Height and width must be odd to use center_radius")
            for i in range(center_y - center_radius, center_y + center_radius + 1):
                for j in range(center_x - center_radius, center_x + center_radius + 1):
                    grid.remove_edge(grid[i, j], grid[i, j + 1])
                    grid.remove_edge(grid[i, j], grid[i + 1, j])
                    grid[i, j].add_tag("center")
        for edge in grid._edges:
            grid._edges[edge] = random.random()
        tree: Grid = grid.prim_algorithm()
        for node1, node2 in list(grid.edges):
            if not tree.is_edge(node1, node2):
                grid.remove_edge(node1, node2)
            else:
                grid.set_weight(node1, node2, 1)
        if center_radius:
            for i in range(center_y - center_radius, center_y + center_radius + 1):
                for j in range(center_x - center_radius, center_x + center_radius + 1):
                    grid.add_edge(grid[i, j], grid[i, j + 1])
                    grid.add_edge(grid[i, j], grid[i + 1, j])
            for i in range(center_y - center_radius, center_y + center_radius + 1):
                if i != center_y:
                    if grid.is_edge(grid[i, center_x - center_radius], grid[i, center_x - center_radius - 1]):
                        grid.remove_edge(grid[i, center_x - center_radius], grid[i, center_x - center_radius - 1])
                    if grid.is_edge(grid[i, center_x + center_radius], grid[i, center_x + center_radius + 1]):
                        grid.remove_edge(grid[i, center_x + center_radius], grid[i, center_x + center_radius + 1])
            for j in range(center_x - center_radius, center_x + center_radius + 1):
                if j != center_x:
                    if grid.is_edge(grid[center_y - center_radius, j], grid[center_y - center_radius - 1, j]):
                        grid.remove_edge(grid[center_y - center_radius, j], grid[center_y - center_radius - 1, j])
                    if grid.is_edge(grid[center_y + center_radius, j], grid[center_y + center_radius + 1, j]):
                        grid.remove_edge(grid[center_y + center_radius, j], grid[center_y + center_radius + 1, j])
        return grid


if __name__ == '__main__':
    g = Grid.make_labyrinth(21, 21, center_radius=2)
    g.show()
    mat = g.to_matrix()
    cmap = colors.ListedColormap(['black', 'white', 'green'])
    plt.imshow(mat, cmap=cmap, origin='lower')
    plt.show()
    cache = []
    OPTIONAL_WALLS = 20
    for i in range(g.height):
        for j in range(g.width):
            if g[i, j].has_tag("center"):
                continue
            if i>0 and not g.is_edge(g[i, j], g[i-1, j]) and not g[i-1, j].has_tag("center"):
                distance = g.distance(g[i, j], g[i-1, j])
                bisect.insort(cache, (distance, (i, j), (i-1, j)))
            if j>0 and not g.is_edge(g[i, j], g[i, j-1]) and not g[i, j-1].has_tag("center"):
                distance = g.distance(g[i, j], g[i, j-1])
                bisect.insort(cache, (distance, (i, j), (i, j-1)))
    c = 0
    first = True
    while c < OPTIONAL_WALLS:
        distance, (i1, j1), (i2, j2) = cache.pop()
        if not first:
            distance = g.distance(g[i1, j1], g[i2, j2])
            if distance < cache[-1][0]:
                bisect.insort(cache, (distance, (i1, j1), (i2, j2)))
                continue
        else:
            first = False
        g.add_edge(g[i1, j1], g[i2, j2])
        x = int(i1 + i2)+1
        y = int(j1 + j2)+1
        mat[x, y] = 3
        c += 1


    cmap = colors.ListedColormap(['black', 'white', 'green', 'red'])
    plt.imshow(mat, cmap=cmap, origin='lower')
    plt.show()
