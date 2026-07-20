<?php
$data = $_GET['payload'];
$unserialized = unserialize($data); // Unsafe PHP injection sink
?>
