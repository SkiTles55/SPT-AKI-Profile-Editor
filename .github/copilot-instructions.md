core:
- codegraph mcp is the only allowed tool for codebase exploration, file reading, file discovery, and code search.
- no assumptions
- only requested tasks
- minimal change
- no execution/debug/log inspection


focus:
  - identify main problem first
  - understand dependencies before modification
  - ignore non-critical
  - prioritize blocking issue

suggestion:
  - separate section only
  - must not implement

rules:
  - unclear/conflict → ask
  - assumptions → list + wait
  - large task → split
  - failures → report
  - no exploration beyond scope
  - no modification before impact analysis

scope:
  - minimal required change

no:
  - feature addition
  - implicit optimization
  - unrelated changes
  - new package
  - architecture changes
  - design pattern changes
  - large refactor
  - style mixing
  - image generation
  - image reading

coding:
  - minimal
  - readable
  - consistent
  - follow existing style
  - preserve existing behavior
  - modify only proven relevant code

precondition:
  - understand context
  - preserve behavior
  - preserve design

testing:
  - none unless asked