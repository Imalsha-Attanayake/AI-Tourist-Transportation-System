# Sri Lankan route network
# Distances are in kilometres

graph = {
    "Colombo": {
        "Kandy": 115,
        "Galle": 125,
        "Negombo": 40
    },

    "Kandy": {
        "Colombo": 115,
        "Nuwara Eliya": 75,
        "Sigiriya": 90
    },

    "Galle": {
        "Colombo": 125,
        "Ella": 230
    },

    "Negombo": {
        "Colombo": 40,
        "Sigiriya": 150
    },

    "Nuwara Eliya": {
        "Kandy": 75,
        "Ella": 55
    },

    "Sigiriya": {
        "Kandy": 90,
        "Negombo": 150,
        "Ella": 200
    },

    "Ella": {
        "Nuwara Eliya": 55,
        "Galle": 230,
        "Sigiriya": 200
    }
}

print("Route graph created successfully!")
print(graph)

import heapq

def dijkstra(graph, start, destination):
    distances = {location: float("inf") for location in graph}
    previous = {location: None for location in graph}

    distances[start] = 0

    priority_queue = [(0, start)]

    while priority_queue:
        current_distance, current_location = heapq.heappop(priority_queue)

        if current_distance > distances[current_location]:
            continue

        if current_location == destination:
            break

        for neighbour, distance in graph[current_location].items():
            new_distance = current_distance + distance

            if new_distance < distances[neighbour]:
                distances[neighbour] = new_distance
                previous[neighbour] = current_location

                heapq.heappush(
                    priority_queue,
                    (new_distance, neighbour)
                )

    # Reconstruct the shortest route
    route = []
    current = destination

    while current is not None:
        route.append(current)
        current = previous[current]

    route.reverse()

    return route, distances[destination]

# Test the route optimisation
start_location = "Colombo"
destination = "Ella"

route, distance = dijkstra(graph, start_location, destination)

print("\nShortest route:")
print(" -> ".join(route))

print(f"Total distance: {distance} km")