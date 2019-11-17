# User Analytics

This section describes Kusto extentions (plugins) for User Analytics scenarios.

|Scenario|Plugin|Details|User Experience|
|--------|------|--------|-------|
| Counting new users over time | [activity_counts_metrics](activity-counts-metrics-plugin.md)|Returns counts/dcounts/new counts for each time window. Each time window is compared to *all* previous time windows|Kusto.Explorer: Report Gallery|
| Period-over-period: retention/churn rate and new users | [activity_metrics](activity-metrics-plugin.md)|Returns dcounts, retention/curn rate for each each time windows. Each time window is compared to *previous* time window|Kusto.Explorer: Report Gallery|
| Users count and dcounts over sliding window | [sliding_window_counts](sliding-window-counts-plugin.md)|For each time window, returns counts and dcounts over a lookback period, in a sliding window manner|
| New-users cohort: retention/churn rate and new users | [new_activity_metrics](new-activity-metrics-plugin.md)|Compares between cohorts of new users (all users which were 1st seen in time window). Each cohort is compared to all prior cohorts. Comparison takes into account *all* previous time windows|Kusto.Explorer: Report Gallery|
|Active Users: distinct counts |[active_users_count](active-users-count-plugin.md)|Returns distinct users for each time window, where a user is only considered if it appears in at least X distinct periods in a spcified lookback period.|
|User Engagement: DAU/WAU/MAU|[activity_engagement](activity-engagement-plugin.md)|Compares between an inner time window (e.g., daily) and an outer (e.g., weekly) for computing engagement (e.g., DAU/WAU)|Kusto.Explorer: Report Gallery|
|Sessions: count active sessions|[session_count](session-count-plugin.md)|Counts sessions, where a session is defined by a time period - a user record is considered a new session, if it has not been seen in the lookback period from current record|
||||
|Funnels: previous and next state sequence analysis | [funnel_sequence](funnel-sequence-plugin.md)|Counts distinct users who have taken a sequence of events, and the prev/next events that led / were followed by the sequence. Useful for constructing [sankey diagrams](https://en.wikipedia.org/wiki/Sankey_diagram)||
|Funnels: sequence completion analysis|[funnel_sequence_completion](funnel-sequence-completion-plugin.md)|Computes the distinct count of users that have *completed* a specified sequence in each time window|
||||
