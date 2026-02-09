## loadfile

Loads a binary file into any RAM component with the L (Load) pin active.

**Usage:**
```
loadfile <filepath>
```

**Example:**
```
loadfile /path/to/program.bin
```

**Note:** This command does not clear memory after the end of the file, allowing partial memory updates.
