"""
Sample: Using the Kusto.Language native AOT library from Python.

This script demonstrates calling the KQL parser through its C FFI surface
using ctypes. No .NET runtime is required — just the compiled native DLL.

Exports used:
    kql_parse(query)           -> "OK" or first error message
    kql_get_diagnostics(query) -> JSON array of diagnostics
    kql_get_syntax_tree(query) -> indented syntax tree dump
    kql_get_syntax_kind(query) -> top-level syntax kind
    kql_free(ptr)              -> free memory returned by the above
"""

import ctypes
import json
import os
import sys

# ---------------------------------------------------------------------------
# Load the native DLL
# ---------------------------------------------------------------------------

DLL_NAME = "Kusto.Language.AOT.dll"
DLL_DIR = os.path.join(
    os.path.dirname(__file__),
    "Kusto.Language.AOT", "bin", "Release", "net9.0", "win-x64", "publish",
)
DLL_PATH = os.path.join(DLL_DIR, DLL_NAME)

if not os.path.exists(DLL_PATH):
    sys.exit(f"DLL not found at {DLL_PATH}\n"
             f"Run: dotnet publish src/Kusto.Language.AOT -r win-x64 -c Release /p:NativeLib=Shared")

lib = ctypes.CDLL(DLL_PATH)

# Declare signatures
lib.kql_parse.argtypes = [ctypes.c_char_p]
lib.kql_parse.restype = ctypes.c_void_p

lib.kql_get_diagnostics.argtypes = [ctypes.c_char_p]
lib.kql_get_diagnostics.restype = ctypes.c_void_p

lib.kql_get_syntax_tree.argtypes = [ctypes.c_char_p]
lib.kql_get_syntax_tree.restype = ctypes.c_void_p

lib.kql_get_syntax_kind.argtypes = [ctypes.c_char_p]
lib.kql_get_syntax_kind.restype = ctypes.c_void_p

lib.kql_free.argtypes = [ctypes.c_void_p]
lib.kql_free.restype = None


# ---------------------------------------------------------------------------
# Helper: call a string->string export, auto-freeing the result
# ---------------------------------------------------------------------------

def _call_str(func, query: str) -> str:
    ptr = func(query.encode("utf-8"))
    try:
        return ctypes.string_at(ptr).decode("utf-8")
    finally:
        lib.kql_free(ptr)


def kql_parse(query: str) -> str:
    """Return 'OK' if the query is syntactically valid, or the first error."""
    return _call_str(lib.kql_parse, query)


def kql_diagnostics(query: str) -> list[dict]:
    """Return a list of diagnostics (start, length, severity, message)."""
    return json.loads(_call_str(lib.kql_get_diagnostics, query))


def kql_syntax_tree(query: str) -> str:
    """Return an indented dump of the syntax tree."""
    return _call_str(lib.kql_get_syntax_tree, query)


def kql_syntax_kind(query: str) -> str:
    """Return the top-level syntax kind of the query."""
    return _call_str(lib.kql_get_syntax_kind, query)


# ---------------------------------------------------------------------------
# Demo
# ---------------------------------------------------------------------------

if __name__ == "__main__":
    queries = [
        # Valid queries
        "StormEvents | where State == 'TEXAS' | count",
        "T | project a = a + b | where a > 10.0",
        "T | summarize count() by bin(Timestamp, 1h)",
        # Invalid query
        "T | where",
    ]

    for query in queries:
        print(f"{'='*60}")
        print(f"Query: {query}")
        print(f"Kind:  {kql_syntax_kind(query)}")

        result = kql_parse(query)
        if result == "OK":
            print(f"Valid: ✓")
        else:
            print(f"Error: {result}")
            for diag in kql_diagnostics(query):
                print(f"  [{diag['severity']}] offset {diag['start']}: {diag['message']}")

        print(f"\nSyntax tree:")
        print(kql_syntax_tree(query))
