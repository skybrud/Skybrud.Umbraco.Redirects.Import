# Columns

When importing either CSV or XLSX files, the package relies on the same logic for mapping the columns of an uploaded file. To support more different scenarios, the importer looks for a few different names for each column.


## Required

If an uploaded file specifies no other columns, the file should at least specify a **Original URL** column as well as a **Destination URL** column.

### Original URL

Specifies the original (inbound) URL of the redirect. The following alises are supported:

- URL
- Old
- 🟥 Old URL *(not supported, but should be)*
- From
- Inbound URL
- Original URL

### Destination URL

Specifies the destination URL of the redirect.

The following names are supported:

- To
- New
- New URL
- Destination URL

The value may include both a query string and/or a fragment, but the query string may also be specified via the [destination query](#destination-query) column and the fragment via the [destination fragment](#destination-fragment) column.


The *destination URL* column itself may also be omitted if the [destionation key][#destination-key] column is specified and the [destination type](#destination-type) column is set to either `Content` or `Media`.


## Optional

### Root Node Key

Specifies the Umbraco ID or GUID key of the root node. Alternatively the value may be a domain, in which case the importer tries to map the domain to an Umbraco root node.

If the column is omitted or set to either `0` or `00000000-0000-0000-0000-000000000000`, the redirect will be created as a global redirect.

The following names are supported:

- Site ID
- Site Key
- Root Node ID
- Root Key
- Root Node Key
- Domain

### Original Query

Specifies the query string of the inbound URL.

The following names are supported:

- Query
- Query String
- Inbound Query
- Inbound Query String
- 🟥 Original Query *(not supported, but should be)*
- 🟥 Original Query String *(not supported, but should be)*

### Destination Key

Specifies the GUID key of a content or media item that represents the destination. Alternatively the value may be the numeric Umbraco ID instead.

If not specified, the import tries to determine the GUID key value of the [destination URL](#destination-url) column instead.

The following names are supported:

- Destination Key
- Destination ID

### Destination Type

Specifies the type of the destination. Supported values are `Content`, `Media` and `Url`.

If not specified, the import tries to determine the type from the value of the [destination URL](#destination-url) column instead.

### Destination Query

Specifies the query string of the destination. The query string may also be part of the [destination URL](#destination-url) column instead.

### Destination Fragment

Specifies the fragment (`#`) of the destination. The fragment may also be part of the [destination URL](#destination-url) column instead.

### Type

Specifies the type of the redirect. Supported values are `Permanent` and `Temporary`. Alternatively a boolean value may be specified instead, where a `true` value equals `Permanent` and `false` equals `Temporary`.

The following names are supported:

- Type
- Redirect Type
- Permanent
- Is permanent

### Forward query string

Specifies whether the query string of the inbound request should be forwarded (copied) to the destination URL.

The following names are supported:

- Forward
- Forward Query
- Forward Query String