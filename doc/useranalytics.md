---
title:  User Analytics
description: This article describes User Analytics in Azure Data Explorer.
ms.reviewer: alexans
ms.topic: reference
ms.date: 11/17/2019
---
# User analytics plugins

This section describes Kusto extensions (plugins) for user analytics scenarios.

|Scenario|Plugin|Details|User Experience|
|--------|------|--------|-------|
| Counting new users over time | [activity_counts_metrics](activity-counts-metrics-plugin.md)|Returns counts/dcounts/new counts for each time window. Each time window is compared to *all* previous time windows|Kusto.Explorer: Report Gallery|
| Period-over-period: retention/churn rate and new users | [activity_metrics](activity-metrics-plugin.md)|Returns `dcount`, retention/churn rate for each time window. Each time window is compared to *previous* time window|Kusto.Explorer: Report Gallery|
| Users count and `dcount` over sliding window | [sliding_window_counts](sliding-window-counts-plugin.md)|For each time window, returns count and `dcount` over a lookback period, in a sliding window manner|
| New-users cohort: retention/churn rate and new users | [new_activity_metrics](new-activity-metrics-plugin.md)|Compares between cohorts of new users (all users that were first seen in time window). Each cohort is compared to all prior cohorts. Comparison takes into account *all* previous time windows|Kusto.Explorer: Report Gallery|
|Active Users: distinct counts |[active_users_count](active-users-count-plugin.md)|Returns distinct users for each time window. A user is only considered if it appears in at least X distinct periods in a specified lookback period.|
|User Engagement: DAU/WAU/MAU|[activity_engagement](activity-engagement-plugin.md)|Compares between an inner time window (for example, daily) and an outer (for example, weekly) for computing engagement (for example, DAU/WAU)|Kusto.Explorer: Report Gallery|
|Sessions: count active sessions|[session_count](session-count-plugin.md)|Counts sessions, where a session is defined by a time period - a user record is considered a new session, if it hasn't been seen in the lookback period from current record|
||||
|Funnels: previous and next state sequence analysis | [funnel_sequence](funnel-sequence-plugin.md)|Counts distinct users who have taken a sequence of events, and the previous or next events that led or were followed by the sequence. Useful for constructing [sankey diagrams](https://en.wikipedia.org/wiki/Sankey_diagram)||
|Funnels: sequence completion analysis|[funnel_sequence_completion](funnel-sequence-completion-plugin.md)|Computes the distinct count of users that have *completed* a specified sequence in each time window|
||||