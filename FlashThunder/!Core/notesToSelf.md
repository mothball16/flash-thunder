(Notepad for learning and stuff)

# Model-View-Presenter pattern
- the model is the ECS world
- the presenter can be the state itself in simple cases (assign it as the interface)
  or a distinct presenter in complex cases
- the view is whatever screen we make in gum
- the view notifies the presenter about user input events, which then updates model/view as necessary 
- the model notifies the presenter about state updates, which then updates the view
- both view and presenter know about each other, but the view does not know about the model
- or the view can just call presenter methods while passing itself in ?

# DefaultECS -> Fennecs
- Get() is Ref()
- Query is built in constructor
- Uniforms are like extra variables or something (need to look into this)

# Bundles v.s. Prefabs
- Bundles are for in-code creation where the UNSET component list is pre-defined 
  (like an interface/contract)
- Prefabs are for data-driven generation where data is pre-defined (like an enemy type)

# The line between ECS and OOP
- ECS should be used for manipulation of world entities/data
- OOP should be used for high-level state management / operations on single instances

# How archetype-based ECS (like fennecs) works
- Every entity with a unique combination of components is stored in a separate archetype
- An archetype is a collection of entities that share the same set of components
- Queries filter over the collection of archetypes, not entities
- Queries store the archetypes that match the query, and lazy update when a new archetype is added
- A complex query is not significantly more expensive than a simple one for this reason ^^

# Why OOP is less performant than ECS for data-heavy operations
- TBA: need to look into this more. something abt irrelevant and scattered lookups, but i don't 
  understand this yet
