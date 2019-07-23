---
title: User Analytics - Azure Data Explorer | Microsoft Docs
description: This article describes User Analytics in Azure Data Explorer.
services: data-explorer
author: orspod
ms.author: orspodek
ms.reviewer: mblythe
ms.service: data-explorer
ms.topic: reference
ms.date: 01/15/2019
---
# User Analytics

This section describes Kusto extentions (plugins) for User Analytics scenarios.

|Scenario|Plugin|User Experience|
|--------|------|---------------|
| Counting new users over time | [activity_counts_metrics](activity-counts-metrics-plugin.md)|Kusto.Explorer: Report Gallery|
| Aggregated new users count over time | [activity_counts_metrics](activity-counts-metrics-plugin.md)|Kusto.Explorer: Report Gallery|
| Users count and dcounts over sliding window | [sliding_window_counts](sliding-window-counts-plugin.md)||
||||
| Period-over-period: retention rate | [activity_metrics](activity-metrics-plugin.md)|Kusto.Explorer: Report Gallery|
| Period-over-period: churn rate | [activity_metrics](activity-metrics-plugin.md)|Kusto.Explorer: Report Gallery|
| Period-over-period: new users| [activity_metrics](activity-metrics-plugin.md)|Kusto.Explorer: Report Gallery|
||||
| New-users cohort: retention rate | [new_activity_metrics](new-activity-metrics-plugin.md)|Kusto.Explorer: Report Gallery|
| New-users cohort: churn rate  | [new_activity_metrics](new-activity-metrics-plugin.md)|Kusto.Explorer: Report Gallery|
| New-users cohort: new users | [new_activity_metrics](new-activity-metrics-plugin.md)|Kusto.Explorer: Report Gallery|
||||
|User Engagement: DAU/MAU|[activity_engagement](activity-engagement-plugin.md)|Kusto.Explorer: Report Gallery|
|User Engagement: DAU/WAU|[activity_engagement](activity-engagement-plugin.md)|Kusto.Explorer: Report Gallery|
|User Engagement: WAU/MAU|[activity_engagement](activity-engagement-plugin.md)|Kusto.Explorer: Report Gallery|
||||
|Active Users: distinct counts |[active_users_count](active-users-count-plugin.md)||
||||
||||
|Sessions: count active sessions|[session_count](session-count-plugin.md)||
||||
|Funnels: previous and next state sequence analysis | [funnel_sequence](funnel-sequence-plugin.md)||
|Funnels: sequence completion analysis|[funnel_sequence_completion](funnel-sequence-completion-plugin.md)||
||||