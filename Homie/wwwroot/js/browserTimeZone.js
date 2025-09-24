window.getBrowserTimeZoneId = () => {
    return Intl.DateTimeFormat().resolvedOptions().timeZone;
}