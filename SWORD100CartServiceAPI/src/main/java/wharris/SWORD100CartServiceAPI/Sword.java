package wharris.SWORD100CartServiceAPI;

import java.time.LocalDate;
import org.springframework.data.annotation.*;
import java.io.Serializable;
import java.util.UUID;

public class Sword implements Serializable {

    private static final long serialVersionUID = 1L;

    @Id
    private UUID swordGuid;

    private String name;

    private String description;

    private LocalDate forgedDate;

    public UUID getSwordGuid() {
        return swordGuid;
    }

    public void setSwordGuid(UUID swordUUID) {
        this.swordGuid = swordUUID;
    }

    public String getName() {
        return name;
    }

    public void setName(String name) {
        this.name = name;
    }

    public String getDescription() {
        return description;
    }

    public void setDescription(String description) {
        this.description = description;
    }

    public LocalDate getForgedDate() {
        return forgedDate;
    }

    public void setForgedDate(LocalDate forgedDate) {
        this.forgedDate = forgedDate;
    }
}
