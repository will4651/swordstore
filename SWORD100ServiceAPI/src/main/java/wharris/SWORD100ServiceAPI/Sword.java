package wharris.SWORD100ServiceAPI;

import java.time.LocalDate;
import java.util.UUID;
import org.springframework.data.annotation.*;
import com.fasterxml.jackson.annotation.*;

public class Sword {

    @Id
    private UUID swordGuid;

    private String name;

    private String description;

    private LocalDate forgedDate;

    public UUID getSwordGuid() {
        return swordGuid;
    }

    public void setSwordGuid(UUID SwordGuid) {
        this.swordGuid = SwordGuid;
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
